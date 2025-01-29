using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Repository;
using SupplierRequestsApp.Domain.Service;

namespace SupplierRequestsApp.Data.Repository;

public class SupplierModelRepository : IModelRepository<Supplier>
{
    private readonly IStorage<Supplier> _storageService = new LocalStorageService<Supplier>();
    private static readonly Type ModelType = typeof(Supplier);

    public Supplier? LoadEntity(Guid entityId)
    {
        return _storageService.LoadEntity(ModelType, entityId.ToString());
    }

    public IEnumerable<Supplier> LoadEntities()
    {
        return _storageService.LoadEntities(ModelType);
    }

    public void AddEntity(Supplier entity)
    {
        _storageService.SaveEntity(entity);
    }

    public void RemoveEntity(Supplier entity)
    {
        _storageService.DropEntity(entity);
    }

    public void EditEntity(Supplier updatedEntity)
    {
        _storageService.UpdateEntity(updatedEntity);
    }
}