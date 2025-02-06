using SupplierRequestsApp.Data;

namespace SupplierRequestsApp.Domain.Models;

public class Storage
{
    private Guid _storageId;
    private List<StoredProduct> _products;

    public Storage(Guid storageId, List<StoredProduct> products)
    {
        _storageId = storageId;
        _products = products;
    }

    public Guid StorageId
    {
        get => _storageId;
        set => _storageId = Validator.RequireGuid(value);
    }

    public List<StoredProduct> Products
    {
        get => _products;
        set => _products = value;
    }
}

public class StoredProduct
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}