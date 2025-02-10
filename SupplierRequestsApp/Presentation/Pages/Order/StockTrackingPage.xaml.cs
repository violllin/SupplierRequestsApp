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
                    _controller.AddProductToCart(selectedProduct.Product, quantity, selectedSupplier.Id);
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
    }
}