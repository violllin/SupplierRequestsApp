namespace SupplierRequestsApp.Domain.Models;

public class StockItem
{
    public Product Product { get; set; }
    public int Quantity { get; set; }
    
    public StockItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }
}