using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Domain.Models;

namespace SupplierRequestsApp.Presentation.Controllers;

using Data.Service;
using Domain.Service;

public class OrdersPageController
{
    public readonly ITableService<Order> Service = new TableService<Order>();

    public OrdersPageController()
    {
        try
        {
            UpdateTable();
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error while updating table. Caused by: {e.Message}");
        }
    }

    public ObservableCollection<Order> Orders { get; set; } = new();

    public void UpdateTable()
    {
        Service.UpdateTable();
        Orders.Clear();
        foreach (var order in Service.Data)
        {
            Orders.Add(order);
        }
    }

    public void AddItem(Order order)
    {
        Service.AddItem(order);
        Orders.Add(order);
        Service.UpdateTable();
    }

    public void DropItem(Order order)
    {
        Service.DropItem(order);
        Orders.Remove(order);
        Service.UpdateTable();
    }
}
