using System.Diagnostics;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Presentation.Controllers;
using SupplierRequestsApp.Util;

namespace SupplierRequestsApp.Presentation.Pages.Order
{
    public partial class StockTrackingPage : ContentPage
    {
        private readonly StockTrackingPageController _controller = new();

        public StockTrackingPage()
        {
            InitializeComponent();
            BindingContext = _controller;
        }

        private async Task<(Domain.Models.Product Product, int quantity, Guid Id, string selectedSupplierName)> ShowNewOrderFields(bool isDeficitProduct = false, StockItem? stockItem = null)
        {
            if (isDeficitProduct)
            {
                ArgumentNullException.ThrowIfNull(stockItem);
                var suppliers = _controller.LoadSuppliers(stockItem.Product.SuppliersId);

                var supplierNames = suppliers.Select(s => s.Name).ToArray();
                var selectedSupplierName =
                    await DisplayActionSheet("Выберите поставщика", "Отмена", null, supplierNames);
                var selectedSupplier = suppliers.FirstOrDefault(s => s.Name == selectedSupplierName);

                if (selectedSupplier == null) throw new SupplierNotFoundException("Не найден поставщик для продукта.");

                var result = await DisplayPromptAsync("Заказ товара",
                    $"Введите количество для {stockItem.Product.Name}:",
                    "OK", "Отмена", "Количество", keyboard: Keyboard.Numeric);

                if (int.TryParse(result, out int quantity) && quantity > 0)
                {
                    return (stockItem.Product, quantity, selectedSupplier.Id, selectedSupplierName);
                }
            }
            else
            {
                var nonDeficitProducts = _controller.LoadNonDeficitProducts();
                var productNames = nonDeficitProducts.Select(p => p.Name).ToArray();
                var selectedProductName = await DisplayActionSheet("Выберите товар", "Отмена", null, productNames);
                var selectedProduct = nonDeficitProducts.FirstOrDefault(p => p.Name == selectedProductName);

                if (selectedProduct == null) throw new ProductNotFoundException("Продукт не найден");

                var suppliers = _controller.LoadSuppliers(selectedProduct.SuppliersId);
                var supplierNames = suppliers.Select(s => s.Name).ToArray();
                var selectedSupplierName = await DisplayActionSheet("Выберите поставщика", "Отмена", null, supplierNames);
                var selectedSupplier = suppliers.FirstOrDefault(s => s.Name == selectedSupplierName);

                if (selectedSupplier == null) throw new SupplierNotFoundException("Не найден поставщик для продукта.");

                var result = await DisplayPromptAsync("Заказ товара", $"Введите количество для {selectedProduct.Name}:", "OK", "Отмена", "Количество", keyboard: Keyboard.Numeric);

                if (int.TryParse(result, out int quantity) && quantity > 0)
                {
                    return (selectedProduct, quantity, selectedSupplier.Id, selectedSupplierName);
                }
            }
            throw new InvalidOperationException("Введите корректное количество!");
        }
        
        private async void OnOrderClicked(object sender, EventArgs e)
        {
            if (sender is not Button { BindingContext: StockItem stockItem }) return;
            try
            {
                var (product, quantity, supplierId, supplierName) = await ShowNewOrderFields(true, stockItem);
                _controller.AddProductToCart(product, quantity, supplierId, supplierName);
            }
            catch (SupplierNotFoundException exception)
            {
                Debug.WriteLine($"Cannot add product to cart. Caused by: {exception.Message}\n{exception.StackTrace}");
                await DisplayAlert("Ошибка при добавление продукта в корзину", exception.Message, "ОК");
            }
        }

        private async void OnRemoveFromCartClicked(object? sender, EventArgs e)
        {
            if (sender is not Button {BindingContext: OrderItem item}) return;

            try
            {
                _controller.DropItemFromCart(item);
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error while drop product from cart. Caused by: {exception.Message}\n{exception.StackTrace}");
                await DisplayAlert("Не удалось удалить продукт из корзины.", exception.Message, "OK");
            }
        }

        private async void OnClearCartClicked(object? sender, EventArgs e)
        {
            try
            {
                _controller.DropCart();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error while drop cart. Caused by: {exception.Message}\n{exception.StackTrace}");
                await DisplayAlert("Не удалось очистить корзину.", exception.Message, "OK");
            }
            
        }

        private async void OnPlaceOrderClicked(object? sender, EventArgs e)
        {
            try
            {
                _controller.PlaceOrder();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error while placing order. Caused by: {exception.Message}");
                await DisplayAlert("Не удалось оформить заказ.", exception.Message, "OK");
            }
        }
        
        private async void OnAddNonDeficitProductToCart_Clicked(object? sender, EventArgs e)
        {
            var (product, quantity, supplierId, supplierName) = await ShowNewOrderFields();
            _controller.AddProductToCart(product, quantity, supplierId, supplierName);
        }
    }
}