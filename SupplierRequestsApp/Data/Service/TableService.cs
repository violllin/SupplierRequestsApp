using SupplierRequestsApp.Domain.Service;

namespace SupplierRequestsApp.Data.Service;

public class TableService<T> : ITableService<T> where T: class
{
    private List<T> _data = [];
    private readonly IStorage<T> _storageService = new LocalStorageService<T>();

    public TableService()
    {
        UpdateTable();
    }

    public List<T> Data => _data;

    public void UpdateTable()
    {
        _data = _storageService.LoadEntities(typeof(T)).ToList();
    }

    public void DropItem(object item)
    {
        _storageService.DropEntity(item as T ?? throw new InvalidOperationException("Invalid item to drop"));
    }

    public void AddItem(object item)
    {
        _storageService.SaveEntity(item as T ?? throw new InvalidOperationException("Invalid item to add"));
    }

    public void EditItem(object item)
    {
        _storageService.UpdateEntity(item as T ?? throw new InvalidOperationException("Invalid item to edit"));
    }
}