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
        _order = _orderService.LoadEntities(typeof(Order))
            .FirstOrDefault(order => order.DeliveryStatus == DeliveryStatus.NotCreated);
    }
    
    private Order CreateDraftOrder(Guid supplierId)
    {
        var order = new Order(Guid.NewGuid(), DateTime.Now, supplierId, new List<OrderItem>(),
            DeliveryStatus.NotCreated, PayStatus.NotPaid);
        _orderService.SaveEntity(order);
        return order;
    }

    private OrderItem CreateOrderItem(Guid orderId, Product product, Guid supplierId, int quantity)
    {
        var newOrderItem = new OrderItem(id: Guid.NewGuid(), orderId: orderId, product: product,
            supplierId: supplierId, quantity: quantity);
        _orderItemService.SaveEntity(newOrderItem);
        return newOrderItem;
    }

    public void AddProduct(Product product, int quantity, Guid supplierId)
    {
        try
        {
            if (_order == null)
            {
                var order = CreateDraftOrder(supplierId: supplierId);
                var newOrderItem = CreateOrderItem(orderId: order.Id, product: product, supplierId: supplierId,
                    quantity: quantity);
                order.AddProductToOrder(newOrderItem);
                _orderItemService.SaveEntity(newOrderItem);
            }
            else
            {
                var newOrderItem = CreateOrderItem(orderId: _order.Id, product: product,
                    supplierId: supplierId, quantity: quantity);
                _order.AddProductToOrder(newOrderItem);
                _orderService.UpdateEntity(_order);
            }
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.WriteLine("Can't find the orders dir. Creating a new one. Caused by: " + e.Message + "\n" + e.StackTrace);
            var order = CreateDraftOrder(supplierId: supplierId);
            var newOrderItem = CreateOrderItem(orderId: order.Id, product: product, supplierId: supplierId,
                quantity: quantity);
            order.AddProductToOrder(newOrderItem);
            _orderService.UpdateEntity(order);
            _orderItemService.SaveEntity(newOrderItem);
        }
        catch (Exception e)
        {
            Debug.WriteLine("Error while adding product to cart: " + e.Message + "\n" + e.StackTrace);
            throw;
        }
    }

    public void DropProduct(Product product)
    {
        var orderItem = _orderItemService.LoadEntities(typeof(OrderItem)).FirstOrDefault(oi => oi.Product == product) ??
                        throw new OrderItemNotFoundException("Товар не найден в корзине.");
        var order = _orderService.LoadEntities(typeof(Order)).FirstOrDefault(o => o.Id == orderItem.OrderId) ??
                    throw new OrderNotFoundException("Заказ не найден.");
        order.OrderProducts.Remove(orderItem);
        _orderService.UpdateEntity(order);
        _orderItemService.DropEntity(orderItem);
    }
}