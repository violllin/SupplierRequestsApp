namespace SupplierRequestsApp.Domain.Models;

public class OrderProduct : OrderItem
{
    private string _productName;

    public OrderProduct(Guid id, Guid orderId, Guid supplierId, string supplierName, Guid productId, int quantity, string productName) : base(id, orderId, supplierId, supplierName, productId, quantity)
    {
        ProductName = productName;
    }

    public string ProductName
    {
        get => _productName;
        set => _productName = value;
    }
}