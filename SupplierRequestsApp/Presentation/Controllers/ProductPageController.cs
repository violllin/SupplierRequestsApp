using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;

namespace SupplierRequestsApp.Presentation.Controllers;

public class ProductsPageController
{
    public readonly ITableService<Product> Service = new TableService<Product>();

    public ProductsPageController()
    {
        try
        {
            UpdateTable();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    public ObservableCollection<Product> Products { get; set; } = [];

    public void UpdateTable()
    {
        Service.UpdateTable();
        Products.Clear();
        foreach (var product in Service.Data)
        {
            Products.Add(product);
        }
    }

    public void AddItem(Product product)
    {
        Service.AddItem(product);
        Products.Add(product);
        Service.UpdateTable();
    }

    public void DropItem(Product productToDrop)
    {
        Service.DropItem(productToDrop);
        Products.Remove(productToDrop);
        Service.UpdateTable();
    }
}