namespace SupplierRequestsApp.Domain.Service;

public interface ITableService<T> where T: class
{
    List<T> Data { get; }
    void UpdateTable();
    void DropItem(object item);
    void AddItem(object item);
    void EditItem(object item);
}