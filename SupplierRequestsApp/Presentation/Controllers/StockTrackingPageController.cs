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

    public ObservableCollection<StockItem> DeficitProducts { get; set; } = [];
    public ObservableCollection<OrderItem> CartProducts { get; set; } = [];

    public StockTrackingPageController()
    {
        try
        {
            UpdateTables();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message + "\n" + e.StackTrace);
        }
    }

    private void UpdateTables()
    {
        DeficitProducts.Clear();
        CartProducts.Clear();

        var cartProducts = new ObservableCollection<OrderItem>(LoadCartProducts());
        foreach (var cartProduct in cartProducts)
        {
            CartProducts.Add(cartProduct);
        }
        var deficitProducts = 
            new ObservableCollection<StockItem>(LoadStockDeficitProducts())
                .Where(dp => CartProducts.All(cp => cp.Product.Id != dp.Product.Id))
            .ToList();
        foreach (var deficitProduct in deficitProducts)
        {
            DeficitProducts.Add(deficitProduct);
        }
    }


    private List<OrderItem> LoadCartProducts()
    {
        return _cartService.LoadCart();
    }
    
    private List<StockItem> LoadStockDeficitProducts()
    {
        var shelves = _productsPageController.LoadShelves(_productsPageController.LoadStorages());
        var stockItems = new List<StockItem>();

        foreach (var shelf in shelves)
        {
            foreach (var slot in shelf.Slots.Where(s => s.Value.HasValue))
            {
                var product = _productService.LoadEntity(typeof(Product), slot.Value.ToString()!);
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

    public void AddProductToCart(Product product, int quantity, Guid supplierId, string supplierName)
    {
        var orderItem = _cartService.AddProduct(product, quantity, supplierId, supplierName);
        DeficitProducts.Remove(DeficitProducts.FirstOrDefault(si => si.Product == product)!);
        CartProducts.Add(orderItem);
    }
    
    public void DropItemFromCart(OrderItem orderItem)
    {
        _cartService.DropItem(orderItem: orderItem);
        UpdateTables();
    }

    public List<Supplier?> LoadSuppliers(List<Guid> supplierIds)
    {
        return supplierIds.Select(id =>
                _supplierService.LoadEntity(typeof(Supplier),
                    id.ToString()))
            .Where(supplier => supplier != null)
            .ToList() ?? throw new SupplierNotFoundException("Не найдено ни одного поставщика для данного товара");
    }

    public void DropCart()
    {
        _cartService.DropCart();
        UpdateTables();
    }
}