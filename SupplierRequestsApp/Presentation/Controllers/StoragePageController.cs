using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;

namespace SupplierRequestsApp.Presentation.Controllers;

public class StoragePageController
{
    public readonly ITableService<Storage> Service = new TableService<Storage>();

    public StoragePageController()
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

    public ObservableCollection<Storage> Storages { get; set; } = [];

    public void UpdateTable()
    {
        Service.UpdateTable();
        Storages.Clear();
        foreach (var itemStorage in Service.Data)
        {
            Storages.Add(itemStorage);
        }
        Debug.WriteLine(Storages.Count);
    }
}