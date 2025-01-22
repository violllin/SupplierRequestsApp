using SupplierRequestsApp.Data;

namespace SupplierRequestsApp.Domain.Models.Product;

public class Product
{
    private Guid _id;
    private string _name;
    private List<Guid> _suppliersId;
    private Guid _storageId;

    public Product(Guid id, string name, List<Guid> suppliersId, Guid storageId)
    {
        _id = id;
        _name = name;
        _suppliersId = suppliersId;
        _storageId = storageId;
    }

    public Guid Id
    {
        get => _id;
        set => _id = Validator.RequireGuid(value);
    }

    public string Name
    {
        get => _name;
        set => _name = Validator.RequireNotBlank(value);
    }

    public List<Guid> SuppliersId
    {
        get => _suppliersId;
        set => _suppliersId = value;
    }

    public Guid StorageId
    {
        get => _storageId;
        set => _storageId = Validator.RequireGuid(value);
    }
}