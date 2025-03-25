namespace SupplierRequestsApp.Domain.Models;

public class OrderProduct : OrderItem
{
    public OrderProduct(Guid id, Guid orderId, Guid supplierId, string supplierName, Guid productId, int quantity,
        string productName) : base(id, orderId, supplierId, supplierName, productId, quantity)
    {
        ProductName = productName;
    }

    public string ProductName { get; set; }
}