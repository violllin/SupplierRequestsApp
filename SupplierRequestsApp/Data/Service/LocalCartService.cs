using System.Diagnostics;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;
using SupplierRequestsApp.Util;

namespace SupplierRequestsApp.Data.Service;

public class LocalCartService : ICartService
{
    private Order? _order;
    private readonly IStorage<Order> _orderService = new LocalStorageService<Order>();
    private readonly IStorage<OrderItem> _orderItemService = new LocalStorageService<OrderItem>();

    public LocalCartService()
    {
        try
        {
            _order = _orderService.LoadEntities(typeof(Order))
                .FirstOrDefault(order => order.DeliveryStatus == DeliveryStatus.NotCreated);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error while load order. Setting as null. Caused by: {e.Message}\n{e.StackTrace}");
            _order = null;
        }
        
    }
    
    private Order CreateDraftOrder(Guid supplierId)
    {
        var order = new Order(Guid.NewGuid(), DateTime.Now, supplierId, new List<OrderItem>(),
            DeliveryStatus.NotCreated, PayStatus.NotPaid);
        _orderService.SaveEntity(order);
        return order;
    }

    private OrderItem CreateOrderItem(Guid orderId, Product product, Guid supplierId, int quantity, string supplierName)
    {
        var newOrderItem = new OrderItem(id: Guid.NewGuid(), orderId: orderId, product: product,
            supplierId: supplierId, quantity: quantity, supplierName: supplierName);
        _orderItemService.SaveEntity(newOrderItem);
        return newOrderItem;
    }

    public OrderItem AddProduct(Product product, int quantity, Guid supplierId, string supplierName)
    {
        try
        {
            _order ??= CreateDraftOrder(supplierId: supplierId);
            var newOrderItem = CreateOrderItem(orderId: _order.Id, product: product,
                supplierId: supplierId, quantity: quantity, supplierName: supplierName);
            _order.AddProductToOrder(newOrderItem);
            _orderService.UpdateEntity(_order);
            return newOrderItem;
        }
        catch (Exception e)
        {
            Debug.WriteLine("Error while adding orderItem to cart: " + e.Message + "\n" + e.StackTrace);
            throw;
        }
    }

    public void DropItem(OrderItem orderItem)
    {
        _order?.OrderProducts.Remove(orderItem);
        _orderService.UpdateEntity(_order!);
        _orderItemService.DropEntity(orderItem);
    }

    public void DropCart()
    {
        if (_order == null) throw new OrderNotFoundException("Корзина не найдена.");
        foreach (var orderItem in _order.OrderProducts)
        {
            _orderItemService.DropEntity(orderItem);
        }
        _order.ClearOrderProducts();
        _orderService.SaveEntity(_order);
    }

    public List<OrderItem> LoadCart()
    {
        return _order != null ? _order.OrderProducts : [];
    }
}