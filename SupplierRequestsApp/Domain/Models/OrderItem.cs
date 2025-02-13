using SupplierRequestsApp.Data;
using SupplierRequestsApp.Domain.Models;

public class OrderItem
{
    private Guid _id;
    private Guid _orderId;
    private Guid _supplierId;
    private string _supplierName;
    private Guid _productId;
    private int _quantity;


    public OrderItem(Guid id, Guid orderId, Guid supplierId, string supplierName, Guid productId, int quantity)
    {
        Id = id;
        OrderId = orderId;
        SupplierId = supplierId;
        SupplierName = supplierName;
        ProductId = productId;
        Quantity = quantity;
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

    public Guid ProductId
    {
        get => _productId;
        set => _productId = Validator.RequireGuid(value);
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