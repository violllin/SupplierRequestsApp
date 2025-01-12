namespace SupplierRequestsApp.Domain.Models;

public class Product
{
    private Guid _id;
    private string _name;

    public Guid Id
    {
        get => _id;
        set => _id = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public Product(Guid id, string name)
    {
        _id = id;
        _name = name;
    }
}