using System.Diagnostics;
using SupplierRequestsApp.Domain.Service;

namespace SupplierRequestsApp.Data.Service;

public class TableService<T> : ITableService<T> where T: class
{
    private readonly IStorage<T> _storageService = new LocalStorageService<T>();

    public TableService()
    {
        UpdateTable();
    }

    public List<T> Data { get; private set; }

    public void UpdateTable()
    {
        try
        {
            Data = _storageService.LoadEntities().ToList();
        }
        catch (DirectoryNotFoundException directoryNotFoundException)
        {
            Debug.WriteLine($"Loaded empty {typeof(T)} table. {directoryNotFoundException.Message}");
            Data = [];
        }
    }

    public void DropItem(object item)
    {
        _storageService.DropEntity(item as T ?? throw new InvalidOperationException("Неизвестный элемент для удаления"));
    }

    public void AddItem(object item)
    {
        _storageService.SaveEntity(item as T ?? throw new InvalidOperationException("Неизвестный элемент для добавления"));
    }

    public void EditItem(object item)
    {
        _storageService.UpdateEntity(item as T ?? throw new InvalidOperationException("Неизвестный элемент для редактирования"));
    }
}