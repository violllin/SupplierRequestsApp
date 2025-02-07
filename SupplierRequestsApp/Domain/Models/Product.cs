using SupplierRequestsApp.Data;

namespace SupplierRequestsApp.Domain.Models;

public class Product
{
    private Guid _id;
    private string _name;
    private List<Guid> _suppliersId;
    private Guid? _shelfId;
    private int? _slotIndex;

    public Product(Guid id, string name, List<Guid> suppliersId, Guid? shelfId = null, int? slotIndex = null)
    {
        _id = id;
        _name = Validator.RequireNotBlank(name);
        _suppliersId = suppliersId;
        _shelfId = shelfId;
        _slotIndex = slotIndex;
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

    public Guid? ShelfId
    {
        get => _shelfId;
        set => _shelfId = value;
    }

    public int? SlotIndex
    {
        get => _slotIndex;
        set => _slotIndex = value;
    }

    public void UpdateStorageLocation(Guid shelfId, int slotIndex)
    {
        _shelfId = shelfId;
        _slotIndex = slotIndex;
    }
}