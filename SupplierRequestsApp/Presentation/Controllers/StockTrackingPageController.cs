using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;
using SupplierRequestsApp.Presentation.Controllers;
using SupplierRequestsApp.Util;

public class StockTrackingPageController
{
    private readonly ProductsPageController _productsPageController = new();
    private readonly IStorage<Product> _productService = new LocalStorageService<Product>();
    private readonly IStorage<Supplier> _supplierService = new LocalStorageService<Supplier>();
    private readonly ICartService _cartService = new LocalCartService();

    public ObservableCollection<StockItem> DeficitProducts { get; set; }

    public StockTrackingPageController()
    {
        try
        {
            UpdateTable();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message + "\n" + e.StackTrace);
        }
    }

    private void UpdateTable()
    {
        var newList = new ObservableCollection<StockItem>(LoadStockDeficitProducts());
        DeficitProducts = newList;
    }

    private List<StockItem> LoadStockDeficitProducts()
    {
        var shelves = _productsPageController.LoadShelves(_productsPageController.LoadStorages());
        var stockItems = new List<StockItem>();

        foreach (var shelf in shelves)
        {
            foreach (var slot in shelf.Slots.Where(s => s.Value.HasValue))
            {
                var product = _productService.LoadEntity(typeof(Product), slot.Value.ToString()!) as Product;
                if (product == null) continue;

                var existingItem = stockItems.FirstOrDefault(item => item.Product == product);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    stockItems.Add(new StockItem(product, 1251));
                }
            }
        }

        return stockItems;
    }

    public void AddProductToCart(Product product, int quantity, Guid supplierId)
    {
        _cartService.AddProduct(product, quantity, supplierId);
        DeficitProducts.Remove(DeficitProducts.FirstOrDefault(si => si.Product == product)!);
    }
    
    public void DropProductFromCart(Product product)
    {
        _cartService.DropProduct(product: product);
        DeficitProducts.Add(new StockItem(product, 1314));
    }

    public List<Supplier?> LoadSuppliers(List<Guid> supplierIds)
    {
        return supplierIds.Select(id =>
                _supplierService.LoadEntity(typeof(Supplier),
                    id.ToString()))
            .Where(supplier => supplier != null)
            .ToList() ?? throw new SupplierNotFoundException("Не найдено ни одного поставщика для данного товара");
    }
}