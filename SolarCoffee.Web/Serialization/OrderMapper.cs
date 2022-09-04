using SolarCoffee.Data.Models;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Serialization
{
    /// <summary>
    /// Handles mapping Order data models to and from related View Models
    /// </summary>
    public class OrderMapper
    {
        /// <summary>
        /// Maps an InvoiceModel view model to a SalesOrder data model
        /// </summary>
        /// <param name="invoice">InvoiceModel invoice</param>
        /// <returns>SalesOrder</returns>
        public static SalesOrder SerializeInvoiceToOrder(InvoiceModel invoice)
        {
            var now = DateTime.Now;

            var salesOrderItems = invoice.LineItems!
                .Select(item => new SalesOrderItem
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    Product = ProductMapper.SerializeProductModel(item.Product!)
                })
                .ToList();

            return new SalesOrder
            {
                SalesOrderItems = salesOrderItems,
                CreatedOn = now,
                UpdatedOn = now
            };
        }

        /// <summary>
        /// Maps a collection of SalesOrders (data) to OrderModels (view models)
        /// </summary>
        /// <param name="orders">IEnumerable<SalesOrder> orders</param>
        /// <returns>List<OrderModel></returns>
        public static List<OrderModel> SerializingOrdersToViewModels(IEnumerable<SalesOrder> orders)
        {
            return orders.Select(order => new OrderModel
            {
                Id = order.Id,
                CreatedOn = order.CreatedOn,
                UpdatedOn = order.UpdatedOn,
                SalesOrderItems = SerializeSalesOrderItems(order.SalesOrderItems!),
                Customer = CustomerMapper.SerializeCustomer(order.Customer!),
                IsPaid = order.IsPaid
            }).ToList();
        }

        /// <summary>
        /// Maps a collection of SalesOrderItems (data) to SalesOrderItemModels (view models)
        /// </summary>
        /// <param name="orderItems">IEnumerable<SalesOrderItem> orderItems</param>
        /// <returns>List<SalesOrderItemModel></returns>
        private static List<SalesOrderItemModel> SerializeSalesOrderItems(IEnumerable<SalesOrderItem> orderItems)
        {
            return orderItems
                .Select(item => new SalesOrderItemModel
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    Product = ProductMapper.SerializeProductModel(item.Product!)
                })
                .ToList();
        }
    }
}
