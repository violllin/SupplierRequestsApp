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
            await Navigation.PushAsync(new StockTrackingPage());
        }

        private async void OnDeliveryClicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new DeliveryPage());
        }
    }
}