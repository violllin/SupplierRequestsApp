using SupplierRequestsApp.Data;

namespace SupplierRequestsApp.Domain.Models;

public class Storage
{
    private Guid _storageId;
    private Dictionary<Guid, int> _products;

    public Storage(Guid storageId, Dictionary<Guid, int> products)
    {
        _storageId = storageId;
        _products = products;
    }

    public Guid StorageId
    {
        get => _storageId;
        set => _storageId = Validator.RequireGuid(value);
    }

    public Dictionary<Guid, int> Products
    {
        get => _products;
        set => _products = value;
    }
}