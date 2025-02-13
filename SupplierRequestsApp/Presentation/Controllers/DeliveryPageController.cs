using System.Collections.ObjectModel;
using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;

namespace SupplierRequestsApp.Presentation.Controllers;

public class DeliveryPageController
{
    private readonly IDeliveryService _deliveryService = new LocalDeliveryService();
    private readonly IStorage<Order> _orderService = new LocalStorageService<Order>();
    private readonly IStorage<Product> _productService = new LocalStorageService<Product>();
    public ObservableCollection<Order> Orders { get; set; } = [];


    public DeliveryPageController()
    {
        UpdateTable();
    }

    private void UpdateTable(bool showAll = false)
    {
        var orders = new ObservableCollection<Order>(GetOrders(showAll));
        Orders.Clear();
        foreach (var order in orders)
        {
            Orders.Add(order);
        }
    }

    public void ForceUpdateTable(bool showAll)
    {
        UpdateTable(showAll);
    }

    private List<Product> LoadProducts(Order order)
    {
        return order.OrderProducts.Select(product => _productService.LoadEntity(product.ProductId.ToString()))
            .OfType<Product>().ToList();
    }
    
    private List<Order> GetOrders(bool showAll = false)
    {
        var entities = _orderService
            .LoadEntities().OrderByDescending(order => order.PayStatus == PayStatus.NotPaid || order.DeliveryStatus != DeliveryStatus.Received).ThenByDescending(order => order.DateCreated);
        return showAll
            ? entities.ToList() : 
            entities.Where(order => order.PayStatus == PayStatus.NotPaid || order.DeliveryStatus != DeliveryStatus.Received)
            .ToList();
    }


    public void PayOrder(Order order)
    {
        _deliveryService.PayOrder(order);
        ForceUpdateTable(false);
    }
    
    public void RefundOrder(Order order)
    {
        _deliveryService.RefundOrder(order);
        ForceUpdateTable(false);
    }
    
    public void ReceiveOrder(Order order)
    {
        _deliveryService.ReceiveOrder(order);
        ForceUpdateTable(false);
    }
    
}