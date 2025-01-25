using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;

namespace SupplierRequestsApp.Data.Service;

public class SupplierTableService : ITableService<SupplierTableService>
{
    private List<Supplier> _data = [];
    private readonly IStorage<Supplier> _supplierService = new LocalStorageService<Supplier>();

    public void UpdateTable()
    {
        _data = _supplierService.LoadEntities(typeof(Supplier)).ToList();
    }

    public void DropItem(object item)
    {
        _supplierService.DropEntity(item as Supplier ?? throw new InvalidOperationException("Invalid item to drop"));
    }

    public void AddItem(object item)
    {
        _supplierService.SaveEntity(item as Supplier ?? throw new InvalidOperationException("Invalid item to add"));
    }

    public void EditItem(object item)
    {
        _supplierService.UpdateEntity(item as Supplier ?? throw new InvalidOperationException("Invalid item to edit"));
    }
}