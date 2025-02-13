using System.Collections.ObjectModel;
using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;

namespace SupplierRequestsApp.Presentation.Controllers;

public class DeliveryPageController
{
    private readonly IDeliveryService _deliveryService = new LocalDeliveryService();
    public ObservableCollection<Order> Orders { get; set; } = [];
    private readonly IStorage<Order> _orderService = new LocalStorageService<Order>();


    public DeliveryPageController()
    {
        UpdateTable();
    }

    private void UpdateTable()
    {
        var orders = new ObservableCollection<Order>(GetOrders());
        Orders.Clear();
        foreach (var order in orders)
        {
            Orders.Add(order);
        }
    }
    
    private List<Order> GetOrders()
    {
        return _orderService.LoadEntities().ToList();
    }

    public void PayOrder(Order order)
    {
        _deliveryService.PayOrder(order);
    }
    
    public void RefundOrder(Order order)
    {
        _deliveryService.RefundOrder(order);
    }
    
    public void ReceiveOrder(Order order)
    {
        _deliveryService.ReceiveOrder(order);
    }
    
}