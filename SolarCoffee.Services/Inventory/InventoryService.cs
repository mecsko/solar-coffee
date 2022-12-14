using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly SolarDbContext _db;
        private readonly ILogger<InventoryService> _logger;
        public InventoryService(SolarDbContext dbContext, ILogger<InventoryService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Returns all current inventory from the database
        /// </summary>
        /// <returns>List<ProductInventory></returns>
        public List<ProductInventory> GetCurrentInventory()
        {
            if (!_db.ProductInventorySnapshots!.Any())
            {
                try
                {
                    CreateSnapshot();
                }
                catch (Exception e)
                {
                    _logger.LogError("Error creating inventory snapshot");
                    _logger.LogError("{}", e.StackTrace);
                }
                _db.SaveChanges();
            }
            
            return _db.ProductInventories!
                .Include(pi => pi.Product)
                .Where(pi => !pi.Product!.IsArchived)
                .ToList();
        }

        /// <summary>
        /// Updates number of units available of the provided product id
        /// Adjusts QuantityOnHand by adjustment value
        /// </summary>
        /// <param name="id">int productId</param>
        /// <param name="adjustment">int number of units added / removed from inventory</param>
        /// <returns>ServiceResponse<ProductInventory></returns>
        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            var now = DateTime.UtcNow;
            try
            {
                var inventory = _db.ProductInventories!
                    .Include(inv => inv.Product)
                    .First(inv => inv.Product!.Id == id);

                inventory.QuantityOnHand += adjustment;

                try
                {
                    CreateSnapshot();
                }
                catch (Exception e)
                {
                    _logger.LogError("Error creating inventory snapshot");
                    _logger.LogError("{}", e.StackTrace);
                }

                _db.SaveChanges();

                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = true,
                    Data = inventory,
                    Message = $"Product {id} inventory adjusted",
                    Time = now
                };
            }
            catch
            {
                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Error updating ProductInventory QuantityOnHand",
                    Time = now
                };
            }
        }

        /// <summary>
        /// Gets a ProductInventory instance by Product ID
        /// </summary>
        /// <param name="productId">int productID</param>
        /// <returns>ProductInventory</returns>
        public ProductInventory GetByProductId(int productId)
        {
            return _db.ProductInventories!
                .Include(pi => pi.Product)
                .FirstOrDefault(pi => pi.Product!.Id == productId)!;
        }

        /// <summary>
        /// Creates a Snapshot record using provided ProductInventory instance
        /// </summary>
        //private void CreateSnapshot(ProductInventory inventory)
        //{
        //    var now = DateTime.UtcNow;

        //    var snapshot = new ProductInventorySnapshot
        //    {
        //        SnapshotTime = now,
        //        Product = inventory.Product,
        //        QuantityOnHand = inventory.QuantityOnHand
        //    };

        //    _db.Add(snapshot);
        //}
        public void CreateSnapshot()
        {
            var now = DateTime.Now;

            var inventories = _db.ProductInventories!
                .Include(inv => inv.Product)
                .ToList();

            foreach (var inventory in inventories)
            {
                var snapshot = new ProductInventorySnapshot
                {
                    SnapshotTime = now,
                    Product = inventory.Product,
                    QuantityOnHand = inventory.QuantityOnHand,
                };

                _db.Add(snapshot);
            }
        }

        /// <summary>
        /// Return Snapshot history for the a previous 6 hours
        /// </summary>
        /// <returns>List<ProductInventorySnapshot></returns>
        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(12);

            return _db.ProductInventorySnapshots!
                .Include(snap => snap.Product)
                .Where(snap 
                    => snap.SnapshotTime > earliest
                       && !snap.Product!.IsArchived)
                .ToList();
        }
    } 
}
