namespace SupplierRequestsApp.Domain.Models;

public class Order
{
    private Guid _id;
    private Guid _supplierId;
    private List<OrderProduct> _orderProducts;
    private OrderStatus _orderStatus;
    private OrderPayStatus _orderPayStatus;

    public Order(Guid id, Guid supplierId, List<OrderProduct> orderProducts, OrderStatus orderStatus, OrderPayStatus orderPayStatus)
    {
        _id = id;
        _supplierId = supplierId;
        _orderProducts = orderProducts;
        _orderStatus = orderStatus;
        _orderPayStatus = orderPayStatus;
    }

    public Guid Id
    {
        get => _id;
        set => _id = value;
    }

    public Guid SupplierId
    {
        get => _supplierId;
        set => _supplierId = value;
    }

    public List<OrderProduct> OrderProducts
    {
        get => _orderProducts;
        set => _orderProducts = value;
    }

    public OrderStatus Status
    {
        get => _orderStatus;
        set => _orderStatus = value;
    }

    public OrderPayStatus PayStatus
    {
        get => _orderPayStatus;
        set => _orderPayStatus = value;
    }
}

public enum OrderStatus
{
    SENT,
    RECEIVED,
    REFUND,
    PAYMENT_WAIT
}
public enum OrderPayStatus
{
    PAID,
    NOT_PAID,
    REFUND
}