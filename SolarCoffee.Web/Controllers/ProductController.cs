using Microsoft.AspNetCore.Mvc;
using SolarCoffee.Data.Models;
using SolarCoffee.Services;
using SolarCoffee.Services.Product;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="product">ProductModel product</param>
        /// <returns>ActionResult(ServiceResponse<Product>)</returns>
        [HttpPost("/api/product")]
        public ActionResult SaveProduct([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            _logger.LogInformation("Adding product");
            var newProduct = ProductMapper.SerializeProductModel(product);
            var newProductResponse = _productService.CreateProduct(newProduct);
            return Ok(newProductResponse);
        }

        /// <summary>
        /// Returns all products
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet("/api/product")]
        public ActionResult GetProduct()
        {
            _logger.LogInformation("Getting all products");
            var products = _productService.GetAllProducts();
            var productViewModels = products.Select(ProductMapper.SerializeProductModel);
            return Ok(productViewModels);
        }

        /// <summary>
        /// Archives an existing product
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns>ActionResult(ServiceResponse<Product>)</returns>
        [HttpPatch("/api/product/{id}")]
        public ActionResult ArchiveProduct(int id)
        {
            _logger.LogInformation("Archiving a product");
            var archiveResult = _productService.ArchiveProduct(id);
            return Ok(archiveResult);
        }
    }
}
