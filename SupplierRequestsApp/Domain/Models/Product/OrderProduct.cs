namespace SupplierRequestsApp.Domain.Models;

public class OrderProduct
{
    private Guid _productId;
    private int count;

    public OrderProduct(Guid productId, int count)
    {
        _productId = productId;
        this.count = count;
    }

    public Guid ProductId
    {
        get => _productId;
        set => _productId = value;
    }

    public int Count
    {
        get => count;
        set => count = value;
    }
}