using SupplierRequestsApp.Domain.Models;

namespace SupplierRequestsApp.Domain.Service;

public interface ICartService
{
    OrderItem AddProduct(Product product, int quantity, Guid supplierId, string supplierName);
    void DropItem(OrderItem orderItem);
    void DropCart(List<OrderItem>? newCartItems = null);
    List<OrderProduct> GetCart();
    List<Order> GetOrders();
    void PlaceOrder();
}