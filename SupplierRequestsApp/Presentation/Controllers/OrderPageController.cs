using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Domain.Models;

namespace SupplierRequestsApp.Presentation.Controllers;

using Data.Service;
using Domain.Service;

public class OrdersPageController
{
    public readonly ITableService<Order> Service = new TableService<Order>();
    private readonly ProductsPageController _productsPageController = new();
    private readonly IStorage<Product> _productService = new LocalStorageService<Product>();

    public OrdersPageController()
    {
        UpdateTable();
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

public class StockItem
{
    public Product Product { get; set; }
    public int Quantity { get; set; }
    
    public StockItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }
}

