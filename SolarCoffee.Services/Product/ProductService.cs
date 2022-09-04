using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly SolarDbContext _db;
        public ProductService(SolarDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Retrives all Product from the database
        /// </summary>
        /// <returns>List<Product></returns>
        public List<Data.Models.Product> GetAllProducts()
        {
            return _db.Products!.ToList();
        }

        /// <summary>
        /// Retrives a Product by primary key
        /// </summary>
        /// <param name="id">int product primary key</param>
        /// <returns>Product</returns>
        public Data.Models.Product GetProductById(int id)
        {
            return _db.Products!.Find(id)!;
        }

        /// <summary>
        /// Add a new product to the database
        /// </summary>
        /// <param name="product">Product instance</param>
        /// <returns>ServiceResponse<Product></returns>
        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _db.Products!.Add(product);

                var newInventory = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10
                };

                _db.ProductInventories!.Add(newInventory);

                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product> 
                { 
                    Data = product,
                    IsSuccess = true,
                    Time = DateTime.UtcNow,
                    Message = "Saved new product"
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                { 
                    Data = product,
                    IsSuccess = false,
                    Time = DateTime.UtcNow,
                    Message = e.StackTrace
                };
            }
        }

        /// <summary>
        /// Archives a Product by setting boolean IsArchived to true
        /// </summary>
        /// <param name="id">int product primary key</param>
        /// <returns>ServiceResponse<Product></returns>
        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            try
            {
                var product = _db.Find<Data.Models.Product>(id);

                product!.IsArchived = true;

                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product> 
                { 
                    Data = product,
                    IsSuccess = true,
                    Time = DateTime.UtcNow,
                    Message = "Archived Product"
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product> 
                { 
                    Data = null,
                    IsSuccess = true,
                    Time = DateTime.UtcNow,
                    Message = e.StackTrace
                };
            }
            
        }

    }
}
