using SupplierRequestsApp.Data;

namespace SupplierRequestsApp.Domain.Models;

public class Storage
{
    private Guid _id;
    private List<Guid> _shelves;

    public Storage(Guid id, List<Guid>? shelves = null)
    {
        _id = id;
        _shelves = shelves ?? new List<Guid>();
    }

    public Guid Id
    {
        get => _id;
        set => _id = Validator.RequireGuid(value);
    }

    public List<Guid> Shelves
    {
        get => _shelves;
        set => _shelves = value;
    }

}


public class StoredProduct
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}