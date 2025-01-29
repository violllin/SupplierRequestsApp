using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Repository;
using SupplierRequestsApp.Domain.Service;

namespace SupplierRequestsApp.Data.Repository;

public class LocalModelRepository<T> : IModelRepository<T> where T : class
{
    private readonly IStorage<T> _storageService = new LocalStorageService<T>();
    private static readonly Type ModelType = typeof(T);

    public T? LoadEntity(Guid entityId)
    {
        return _storageService.LoadEntity(ModelType, entityId.ToString());
    }

    public IEnumerable<T> LoadEntities()
    {
        return _storageService.LoadEntities(ModelType);
    }

    public void AddEntity(T entity)
    {
        _storageService.SaveEntity(entity);
    }

    public void RemoveEntity(T entity)
    {
        _storageService.DropEntity(entity);
    }

    public void EditEntity(T updatedEntity)
    {
        _storageService.UpdateEntity(updatedEntity);
    }
}