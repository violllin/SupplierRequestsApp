using SupplierRequestsApp.Data;

namespace SupplierRequestsApp.Domain.Models;

public class Product
{
    private Guid _id;
    private string _name;
    private List<Guid> _suppliersId;
    private Guid _shelfId;
    private Guid _previosShelfId;
    private List<Guid> _previosSuppliersId;

    public Product(Guid id, string name, List<Guid> suppliersId, Guid shelfId, Guid previosShelfId, List<Guid> previosSuppliersId)
    {
        _id = id;
        _name = name;
        _suppliersId = suppliersId;
        _shelfId = shelfId;
        _previosShelfId = previosShelfId;
        _previosSuppliersId = previosSuppliersId;
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

    public Guid ShelfId
    {
        get => _shelfId;
        set => _shelfId = value;
    }

    public List<Guid> PreviosSuppliersId
    {
        get => _previosSuppliersId;
        set => _previosSuppliersId = value;
    }

    public Guid PreviosShelfId
    {
        get => _previosShelfId;
        set => _previosShelfId = value;
    }
}