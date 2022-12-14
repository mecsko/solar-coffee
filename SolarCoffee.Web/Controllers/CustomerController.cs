using Microsoft.AspNetCore.Mvc;
using SolarCoffee.Services.Customer;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<OrderController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpPost("/api/customer")]
        public ActionResult CreateCustomer([FromBody] CustomerModel customer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var now = DateTime.UtcNow;
            _logger.LogInformation("Creating a new customer");

            customer.CreatedOn = now;
            customer.UpdatedOn = now;

            var customerData = CustomerMapper.SerializeCustomer(customer);
            var newCustomer = _customerService.CreateCustomer(customerData);

            return Ok(newCustomer);
        }

        [HttpGet("/api/customer")]
        public ActionResult GetCustomer()
        {
            var customers = _customerService.GetAllCustomers();
            var customerModels = customers
                .Select(customer => new CustomerModel
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PrimaryAddress = CustomerMapper
                        .MapCustomerAddress(customer.PrimaryAddress!),
                    CreatedOn = customer.CreatedOn,
                    UpdatedOn = customer.UpdatedOn
                })
                .OrderByDescending(customer => customer.CreatedOn)
                .ToList();
            return Ok(customerModels);
        }

        [HttpDelete("/api/customer/{id}")]
        public ActionResult DeleteCustomer(int id)
        {
            _logger.LogInformation("Deleting a customer");
            var response = _customerService
                .DeleteCustomer(id);
            return Ok(response);
        }
    }
}
