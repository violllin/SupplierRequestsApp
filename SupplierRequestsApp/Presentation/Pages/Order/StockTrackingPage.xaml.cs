using System.Diagnostics;
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

        private async void OnOrderClicked(object sender, EventArgs e)
        {
            if (sender is not Button { BindingContext: StockItem selectedProduct }) return;

            try
            {
                var suppliers = _controller.LoadSuppliers(selectedProduct.Product.SuppliersId);

                var supplierNames = suppliers.Select(s => s.Name).ToArray();
                var selectedSupplierName =
                    await DisplayActionSheet("Выберите поставщика", "Отмена", null, supplierNames);
                var selectedSupplier = suppliers.FirstOrDefault(s => s.Name == selectedSupplierName);

                if (selectedSupplier == null) return;

                var result = await DisplayPromptAsync("Заказ товара",
                    $"Введите количество для {selectedProduct.Product.Name}:",
                    "OK", "Отмена", "Количество", keyboard: Keyboard.Numeric);

                if (int.TryParse(result, out int quantity) && quantity > 0)
                {
                    _controller.AddProductToCart(selectedProduct.Product, quantity, selectedSupplier.Id, selectedSupplierName);
                }
                else
                {
                    await DisplayAlert("Ошибка", "Введите корректное количество!", "ОК");
                }
            }
            catch (SupplierNotFoundException exception)
            {
                Debug.WriteLine($"Cannot add product to cart. Caused by: {exception.Message}\n{exception.StackTrace}");
                await DisplayAlert("Ошибка", exception.Message, "ОК");
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
    }
}