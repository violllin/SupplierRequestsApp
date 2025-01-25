namespace SupplierRequestsApp.Domain.Service;

public interface IStorage<T> where T : class
{
    IEnumerable<T> LoadEntities(string dir);
    T? LoadEntity(string dir);
    void SaveEntity(string directoryPath, T entity);
    void UpdateEntity(string dir, T updatedEntity);
}