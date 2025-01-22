using SupplierRequestsApp.Data;
using SupplierRequestsApp.Domain.Models.Product;

namespace SupplierRequestsApp.Domain.Models;

public class Order
{
    private Guid _id;
    private DateTime _dateCreated;
    private Guid _supplierId;
    private List<OrderItem> _orderProducts;
    private DeliveryStatus _deliveryStatus;
    private OrderPayStatus _orderPayStatus;

    public Order(Guid id, DateTime dateCreated, Guid supplierId, List<OrderItem> orderProducts, DeliveryStatus deliveryStatus, OrderPayStatus orderPayStatus)
    {
        Id = id;
        DateCreated = dateCreated;
        SupplierId = supplierId;
        OrderProducts = orderProducts;
        Status = deliveryStatus;
        PayStatus = orderPayStatus;
    }

    public Guid Id
    {
        get => _id;
        set => _id = Validator.RequireGuid(value);
    }

    public Guid SupplierId
    {
        get => _supplierId;
        set => _supplierId = Validator.RequireGuid(value);
    }

    public List<OrderItem> OrderProducts
    {
        get => _orderProducts;
        set => _orderProducts = value;
    }

    public DeliveryStatus Status
    {
        get => _deliveryStatus;
        set => _deliveryStatus = value;
    }

    public OrderPayStatus PayStatus
    {
        get => _orderPayStatus;
        set => _orderPayStatus = value;
    }

    public DateTime DateCreated
    {
        get => _dateCreated;
        set => _dateCreated = value;
    }
}

public enum DeliveryStatus
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