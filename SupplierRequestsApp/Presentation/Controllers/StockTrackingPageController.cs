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
    private readonly IStorage<Order> _orderService = new LocalStorageService<Order>();
    private readonly IStorage<OrderItem> _orderItemService = new LocalStorageService<OrderItem>();
    private readonly IStorage<Supplier> _supplierService = new LocalStorageService<Supplier>();

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
        Debug.WriteLine($"Вызван AddProductToCart: {product.Name}, {quantity}, {supplierId}");
        try
        {
            var orders = _orderService.LoadEntities(typeof(Order)).ToList();
            Debug.WriteLine($"Загружено заказов: {orders.Count}");
            if (orders.Count == 0)
            {
                Debug.WriteLine("Создаю новый заказ...");
                var order = new Order(Guid.NewGuid(), DateTime.Now, supplierId, new List<OrderItem>(),
                    DeliveryStatus.NotCreated, PayStatus.NotPaid);
                var newOrderItem = new OrderItem(id: Guid.NewGuid(), orderId: order.Id, product: product,
                    supplierId: supplierId, quantity: quantity);
                order.AddProductToOrder(newOrderItem);
                _orderService.SaveEntity(order);
                _orderItemService.SaveEntity(newOrderItem);
            }
            else
            {
                var order = orders.FirstOrDefault(o => o.DeliveryStatus == DeliveryStatus.NotCreated);
                if (order == null)
                {
                    Debug.WriteLine("Не найден существующий заказ, создаю новый...");
                    order = new Order(Guid.NewGuid(), DateTime.Now, supplierId, new List<OrderItem>(),
                        DeliveryStatus.NotCreated, PayStatus.NotPaid);
                    _orderService.SaveEntity(order);
                }

                Debug.WriteLine($"Добавляю товар в заказ {order.Id}");
                var newOrderItem = new OrderItem(id: Guid.NewGuid(), orderId: order.Id, product: product,
                    supplierId: supplierId, quantity: quantity);
                order.AddProductToOrder(newOrderItem);
                _orderService.UpdateEntity(order);
                _orderItemService.SaveEntity(newOrderItem);
            }
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.WriteLine(e.Message);
            Debug.WriteLine("Не найден существующий заказ, создаю новый...");
            var order = new Order(Guid.NewGuid(), DateTime.Now, supplierId, new List<OrderItem>(),
                DeliveryStatus.NotCreated, PayStatus.NotPaid);
            _orderService.SaveEntity(order);
            Debug.WriteLine($"Добавляю товар в заказ {order.Id}");
            var newOrderItem = new OrderItem(id: Guid.NewGuid(), orderId: order.Id, product: product,
                supplierId: supplierId, quantity: quantity);
            order.AddProductToOrder(newOrderItem);
            _orderService.UpdateEntity(order);
            _orderItemService.SaveEntity(newOrderItem);
        }
        var updatedOrders = _orderService.LoadEntities(typeof(Order)).ToList();
        Debug.WriteLine($"После сохранения заказов: {updatedOrders.Count}");
        Debug.WriteLine($"Сношу товар из дефицита: {product.Name}");
        DeficitProducts.Remove(DeficitProducts.FirstOrDefault(si => si.Product == product)!);
    }


    public void DropProductFromCart(Product product)
    {
        var orderItem = _orderItemService.LoadEntities(typeof(OrderItem)).FirstOrDefault(oi => oi.Product == product) ??
                        throw new OrderItemNotFoundException("Товар не найден в корзине.");
        var order = _orderService.LoadEntities(typeof(Order)).FirstOrDefault(o => o.Id == orderItem.OrderId) ??
                    throw new OrderNotFoundException("Заказ не найден.");
        order.OrderProducts.Remove(orderItem);
        _orderService.UpdateEntity(order);
        _orderItemService.DropEntity(orderItem);
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