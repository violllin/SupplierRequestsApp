using SupplierRequestsApp.Data;

namespace SupplierRequestsApp.Domain.Models;

public class Supplier
{
    private Guid _id;
    private string _name;
    private string _address;
    private string _phone;
    private List<Guid> _products;

    public Supplier(Guid id, string name, string address, string phone, List<Guid> products)
    {
        Id = id;
        Name = name;
        Address = address;
        Phone = phone;
        Products = products;
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

    public string Address
    {
        get => _address;
        set => _address = Validator.RequireNotBlank(value);
    }

    public string Phone
    {
        get => _phone;
        set => _phone = Validator.RequireNotBlank(value);
    }

    public List<Guid> Products
    {
        get => _products;
        set => _products = value;
    }
}