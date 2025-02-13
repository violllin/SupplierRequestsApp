using System.Diagnostics;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;
using SupplierRequestsApp.Util;

namespace SupplierRequestsApp.Data.Service;

public class LocalCartService : ICartService
{
    private Order? _order;
    private List<Order>? _orders;
    private readonly IStorage<Order> _orderService = new LocalStorageService<Order>();
    private readonly IStorage<OrderItem> _orderItemService = new LocalStorageService<OrderItem>();
    private readonly IStorage<Product> _productService = new LocalStorageService<Product>();

    public LocalCartService()
    {
        _order = LoadOrder();
        _orders = LoadOrders();
    }

    private Order? LoadOrder()
    {
        try
        {
            return _orderService.LoadEntities()
                .FirstOrDefault(order => order.DeliveryStatus == DeliveryStatus.NotCreated);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Can't load order. Caused by: {e.Message}");
            return null;
        }
    }


    private List<Order>? LoadOrders()
    {
        try
        {
            return _orderService.LoadEntities().ToList();
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Can't load orders. Caused by: {e.Message}");
            return null;
        }
    }
    
    private Order CreateDraftOrder(Guid supplierId)
    {
        var loadedOrder = _orderService.LoadEntities()
            .FirstOrDefault(order => order.DeliveryStatus == DeliveryStatus.Created || order.DeliveryStatus == DeliveryStatus.NotCreated);
        if (loadedOrder != null)
        {
            _order = loadedOrder;
            return _order;
        }
        var order = new Order(Guid.NewGuid(), DateTime.Now, supplierId, new List<OrderItem>(),
            DeliveryStatus.NotCreated, PayStatus.NotPaid);
        _orderService.SaveEntity(order);
        return order;
    }

    private OrderItem CreateOrderItem(Guid orderId, Product product, Guid supplierId, int quantity, string supplierName)
    {
        var newOrderItem = new OrderItem(id: Guid.NewGuid(), orderId: orderId, productId: product.Id,
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
        _order = null;
    }

    public List<OrderProduct> GetCart()
    {
        List<OrderProduct> list = [];
        if (_order == null) return list;
        foreach (var item in _order.OrderProducts)
        {
            var product = _productService.LoadEntity(item.ProductId.ToString());
            if (product != null) list.Add(new OrderProduct(item.Id, item.OrderId, item.SupplierId, item.SupplierName,
                item.ProductId, item.Quantity, product.Name));
        }
        return list;
    }

    public List<Order> GetOrders()
    {
        return _orders ?? [];
    }

    public void PlaceOrder()
    {
        if (_order == null) throw new OrderNotFoundException("Для начала заполните корзину.");
        if (_order.OrderProducts.Count == 0)
            throw new PlacingOrderWithEmptyProductsException("Для начала заполните козину.");
        _order.DeliveryStatus = DeliveryStatus.Created;
        _orderService.UpdateEntity(_order);
        _order = null;
        _orders = LoadOrders();
    }
}