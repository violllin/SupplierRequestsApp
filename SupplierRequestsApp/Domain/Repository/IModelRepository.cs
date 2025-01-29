namespace SupplierRequestsApp.Domain.Repository;

public interface IModelRepository<T> where T: class
{
    public T? LoadEntity(Guid entityId);
    public IEnumerable<T> LoadEntities();
    public void AddEntity(T entity);
    public void RemoveEntity(T entity);
    public void EditEntity(T updatedEntity);
}