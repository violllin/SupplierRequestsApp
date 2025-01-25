namespace SupplierRequestsApp.Domain.Service;

public interface IStorage<T> where T : class
{
    IEnumerable<T> LoadEntities(Type type);
    T? LoadEntity(Type type, string fileName);
    void SaveEntity(T entity);
    void UpdateEntity(T updatedEntity);
    void DropEntity(T entity);
}