using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;

namespace SupplierRequestsApp.Presentation.Controllers;

public class SuppliersPageController
{
    public readonly ITableService<Supplier> Service = new TableService<Supplier>();

    public SuppliersPageController()
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

    public ObservableCollection<Supplier> Suppliers { get; set; } = [];

    public void UpdateTable()
    {
        Service.UpdateTable();
        Suppliers.Clear();
        foreach (var supplier in Service.Data)
        {
            Suppliers.Add(supplier);
        }
    }

    public void AddItem(Supplier supplier)
    {
        Service.AddItem(supplier);
        Suppliers.Add(supplier);
        Service.UpdateTable();
    }

    public void DropItem(Supplier supplierToDrop)
    {
        Service.DropItem(supplierToDrop);
        Suppliers.Remove(supplierToDrop);
        Service.UpdateTable();
    }
}