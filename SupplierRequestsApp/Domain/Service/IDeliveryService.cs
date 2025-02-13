using SupplierRequestsApp.Domain.Models;

namespace SupplierRequestsApp.Domain.Service;

public interface IDeliveryService
{
    void PayOrder(Order order);
    void RefundOrder(Order order);
    void ReceiveOrder(Order order);
}