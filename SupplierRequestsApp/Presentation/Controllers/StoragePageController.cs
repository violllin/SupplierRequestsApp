using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;

namespace SupplierRequestsApp.Presentation.Controllers;

public class StoragePageController
{
    public readonly ITableService<Storage> Service = new TableService<Storage>();
    private readonly IStorage<Shelf> _shelfService = new LocalStorageService<Shelf>();
    public ObservableCollection<Storage> Storages { get; set; } = [];

    public StoragePageController()
    {
        UpdateTable();
    }

    public void UpdateTable()
    {
        Service.UpdateTable();
        Storages.Clear();
        foreach (var itemStorage in Service.Data)
        {
            Storages.Add(itemStorage);
        }
    }

    public void AddItem(Storage storage)
    {
        Service.AddItem(storage);
        Storages.Add(storage);
        Service.UpdateTable();
    }

    public void DropItem(Storage storage)
    {
        foreach (var loadedShelf in storage.Shelves.Select(shelf => _shelfService.LoadEntity(shelf.ToString())).OfType<Shelf>())
        {
            DropShelf(loadedShelf);
        }
        Service.DropItem(storage);
        Storages.Remove(storage);
        Service.UpdateTable();
    }

    public void DropShelf(Shelf shelf)
    {
        _shelfService.DropEntity(shelf);
    }
    
    public void AddShelf(Shelf shelf)
    {
        _shelfService.SaveEntity(shelf);
    }
    
    public List<Shelf> GetShelves(Guid storageId)
    {
        return _shelfService.LoadEntities().Where(shelf => shelf.StorageId == storageId).ToList();
    }

    public void UpdateShelf(Shelf shelf)
    {
        _shelfService.UpdateEntity(shelf);
    }
    
}