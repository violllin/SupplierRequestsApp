using SupplierRequestsApp.Data;

namespace SupplierRequestsApp.Domain.Models;

public class Storage
{
    private Guid _storageId;
    private Dictionary<int, int> _products;

    public Storage(Guid storageId, Dictionary<int, int> products)
    {
        _storageId = storageId;
        _products = products;
    }

    public Guid StorageId
    {
        get => _storageId;
        set => _storageId = Validator.RequireGuid(value);
    }

    public Dictionary<int, int> Products
    {
        get => _products;
        set => _products = value;
    }
}