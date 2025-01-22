namespace SupplierRequestsApp.Domain.Models.Product;

public class OrderItem
{
    private Guid _orderId;
    private Guid _productId;
    private int _count;

    public OrderItem(Guid orderId, Guid productId, int count)
    {
        _orderId = orderId;
        _productId = productId;
        _count = count;
    }

    public Guid ProductId
    {
        get => _productId;
        set => _productId = value;
    }

    public int Count
    {
        get => _count;
        set => _count = value;
    }

    public Guid OrderId
    {
        get => _orderId;
        set => _orderId = value;
    }
}