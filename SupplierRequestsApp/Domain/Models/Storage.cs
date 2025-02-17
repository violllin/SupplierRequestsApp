using SupplierRequestsApp.Data;

namespace SupplierRequestsApp.Domain.Models;

public class Storage
{
    private Guid _id;
    private List<Guid> _shelves;

    public Storage(Guid id)
    {
        Id = id;
        Shelves = new List<Guid>();
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