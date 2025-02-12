using SupplierRequestsApp.Data;
using SupplierRequestsApp.Domain.Models;

public class OrderItem
{
    private Guid _id;
    private Guid _orderId;
    private Guid _supplierId;
    private string _supplierName;
    private Product _product;
    private int _quantity;


    public OrderItem(Guid id, Guid orderId, Guid supplierId, string supplierName, Product product, int quantity)
    {
        _id = id;
        _orderId = orderId;
        _supplierId = supplierId;
        _supplierName = supplierName;
        _product = product;
        _quantity = quantity;
    }

    public Guid Id
    {
        get => _id;
        set => _id = Validator.RequireGuid(value);
    }

    public Guid OrderId
    {
        get => _orderId;
        set => _orderId = Validator.RequireGuid(value);
    }

    public Guid SupplierId
    {
        get => _supplierId;
        set => _supplierId = Validator.RequireGuid(value);
    }

    public Product Product
    {
        get => _product;
        set => _product = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Quantity
    {
        get => _quantity;
        set => _quantity = Validator.RequireGreaterOrEqualsThan(value, 1);
    }

    public string SupplierName
    {
        get => _supplierName;
        set => _supplierName = Validator.RequireNotBlank(value);
    }
}