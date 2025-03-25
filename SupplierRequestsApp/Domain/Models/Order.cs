using SupplierRequestsApp.Data;
using SupplierRequestsApp.Util;

namespace SupplierRequestsApp.Domain.Models;

public class Order
{
    private Guid _id;
    private DateTime _dateCreated;
    private Guid _supplierId;
    private List<OrderItem> _orderProducts;
    private DeliveryStatus _deliveryStatus;
    private PayStatus _payStatus;

    public Order(Guid id, DateTime dateCreated, Guid supplierId, List<OrderItem> orderProducts, DeliveryStatus deliveryStatus, PayStatus payStatus)
    {
        Id = id;
        DateCreated = dateCreated;
        SupplierId = supplierId;
        OrderProducts = orderProducts;
        DeliveryStatus = deliveryStatus;
        PayStatus = payStatus;
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

    public DeliveryStatus DeliveryStatus
    {
        get => _deliveryStatus;
        set => _deliveryStatus = value;
    }

    public PayStatus PayStatus
    {
        get => _payStatus;
        set => _payStatus = value;
    }

    public DateTime DateCreated
    {
        get => _dateCreated;
        set => _dateCreated = value;
    }
    
    public void AddProductToOrder(OrderItem orderItem)
    {
        OrderProducts.Add(orderItem);
    }
    
    public void DropProductFromOrder(OrderItem orderItem)
    {
        var item = OrderProducts.Find(item => item.Id == orderItem.Id);
        if (item == null) throw new OrderItemNotFoundException("Объект не найден.");
        OrderProducts.Remove(item);
    }

    public void ClearOrderProducts()
    {
        OrderProducts.Clear();
    }
}

public enum DeliveryStatus
{
    NotCreated = 0, 
    Created,
    InDelivery,
    Delivered,
    Received,
    Refund
}

public enum PayStatus
{
    Paid,
    NotPaid,
    Refund
}