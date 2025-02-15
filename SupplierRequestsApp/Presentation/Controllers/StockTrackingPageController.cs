using System.Collections.ObjectModel;
using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;
using SupplierRequestsApp.Util;

namespace SupplierRequestsApp.Presentation.Controllers;

public class StockTrackingPageController
{
    private readonly ProductsPageController _productsPageController = new();
    private readonly IStorage<Product> _productService = new LocalStorageService<Product>();
    private readonly IStorage<Supplier> _supplierService = new LocalStorageService<Supplier>();
    private readonly ICartService _cartService = new LocalCartService();

    public ObservableCollection<StockItem> DeficitProducts { get; set; } = [];
    public ObservableCollection<OrderProduct> CartProducts { get; set; } = [];

    public StockTrackingPageController()
    {
        UpdateTables();
    }

    private void UpdateTables()
    {
        DeficitProducts.Clear();
        CartProducts.Clear();

        var cartProducts = new ObservableCollection<OrderProduct>(LoadCartProducts());
        foreach (var cartProduct in cartProducts)
        {
            CartProducts.Add(cartProduct);
        }

        var orders = LoadOrders();
        var excludedStatuses = new[] { DeliveryStatus.NotCreated, DeliveryStatus.Refund, DeliveryStatus.Received };
        var activeOrderProducts = orders
            .Where(order => !excludedStatuses.Contains(order.DeliveryStatus))
            .SelectMany(order => order.OrderProducts)
            .Select(item => item.ProductId)
            .ToHashSet();

        var deficitProducts = new ObservableCollection<StockItem>(LoadStockDeficitProducts())
            .Where(dp => !activeOrderProducts.Contains(dp.Product.Id) &&
                         CartProducts.All(cp => cp.ProductId != dp.Product.Id))
            .ToList();

        foreach (var deficitProduct in deficitProducts)
        {
            DeficitProducts.Add(deficitProduct);
        }
    }

    private List<OrderProduct> LoadCartProducts()
    {
        return _cartService.GetCart();
    }

    private List<Order> LoadOrders()
    {
        return _cartService.GetOrders();
    }

    private List<StockItem> LoadStockDeficitProducts()
    {
        var shelves = _productsPageController.LoadShelves(_productsPageController.LoadStorages());
        var stockItems = new List<StockItem>();

        var orders = LoadOrders();
        var undeliveredOrderProducts = orders
            .Where(order => order.DeliveryStatus != DeliveryStatus.Received)
            .SelectMany(order => order.OrderProducts)
            .Select(item => item.ProductId)
            .ToHashSet();

        foreach (var shelf in shelves)
        {
            foreach (var slot in shelf.Slots)
            {
                if (!slot.Value.HasValue) continue;

                var product = _productService.LoadEntity(slot.Value.ToString()!);
                if (product == null || undeliveredOrderProducts.Contains(product.Id)) continue;

                var item = stockItems.FirstOrDefault(item => item.Product.Id == product.Id);
                if (item != null) continue;

                var productCount = shelf.Slots.Count(s => s.Value == product.Id);
                if (productCount < 15) stockItems.Add(new StockItem(product, productCount));
            }
        }

        var allProducts = _productService.LoadEntities();
        foreach (var product in allProducts)
        {
            if (stockItems.Any(item => item.Product.Id == product.Id) ||
                undeliveredOrderProducts.Contains(product.Id)) continue;

            var productCount = shelves.Sum(shelf => shelf.Slots.Count(slot => slot.Value == product.Id));
            if (productCount == 0) stockItems.Add(new StockItem(product, productCount));
        }

        return stockItems;
    }

    public void AddProductToCart(Product product, int quantity, Guid supplierId, string supplierName)
    {
        var orderItem = _cartService.AddProduct(product, quantity, supplierId, supplierName);
        DeficitProducts.Remove(DeficitProducts.FirstOrDefault(si => si.Product == product)!);
        CartProducts.Add(new OrderProduct(orderItem.Id, orderItem.OrderId, orderItem.SupplierId, orderItem.SupplierName,
            orderItem.ProductId, orderItem.Quantity, product.Name));
    }

    public void DropItemFromCart(OrderItem orderItem)
    {
        _cartService.DropItem(orderItem: orderItem);
        UpdateTables();
    }

    public List<Supplier?> LoadSuppliers(List<Guid> supplierIds)
    {
        return supplierIds.Select(id =>
                _supplierService.LoadEntity(id.ToString()))
            .Where(supplier => supplier != null)
            .ToList() ?? throw new SupplierNotFoundException("Не найдено ни одного поставщика для данного товара");
    }

    public void DropCart()
    {
        var newCartItems = CartProducts.Select(cp => new OrderItem(
            id: cp.Id, orderId: cp.OrderId, supplierId: cp.SupplierId, supplierName: cp.SupplierName,
            productId: cp.ProductId, quantity: cp.Quantity)
        ).ToList();

        _cartService.DropCart(newCartItems);
        UpdateTables();
    }

    public void PlaceOrder()
    {
        _cartService.PlaceOrder();
        UpdateTables();
    }

    public List<Product> LoadNonDeficitProducts()
    {
        var allProducts = _productService.LoadEntities();
        var deficitProductIds = DeficitProducts.Select(dp => dp.Product.Id).ToHashSet();
        var cartProductIds = CartProducts.Select(cp => cp.ProductId).ToHashSet();
        return allProducts
            .Where(product => !deficitProductIds.Contains(product.Id) && !cartProductIds.Contains(product.Id)).ToList();
    }
}