using SupplierRequestsApp.Domain.Models;

namespace SupplierRequestsApp.Domain.Service;

public interface ICartService
{
    void AddProduct(Product product, int quantity, Guid supplierId);
    void DropProduct(Product product);
}