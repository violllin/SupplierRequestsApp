using System.Diagnostics;

namespace SupplierRequestsApp.Presentation.Pages.Order
{
    public partial class OrdersPage : ContentPage
    {
        public OrdersPage()
        {
            InitializeComponent();
        }

        private async void OnStockTrackingClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new StockTrackingPage());
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error while open StockTracking page. Caused by: {exception.Message}\n{exception.StackTrace}");
                await DisplayAlert("Не удалось открыть страницу учета товаров.", exception.Message, "OK");
            }
        }

        private async void OnDeliveryClicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new DeliveryPage());
        }
    }
}