using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Presentation.Controllers;
using SupplierRequestsApp.Util;

namespace SupplierRequestsApp.Presentation.Pages.Order
{
    public partial class OrderPage : ContentPage
    {
        public Domain.Models.Order Order { get; set; }
        private DeliveryPageController _controller;
        public ObservableCollection<OrderItem> OrderItems { get; set; }

        public OrderPage(Domain.Models.Order order, DeliveryPageController controller)
        {
            BindingContext = this;
            SetupPage(order, controller);
            InitializeComponent();
            LoadOrderData();
        }

        private void SetupPage(Domain.Models.Order order, DeliveryPageController controller)
        {
            Order = order;
            _controller = controller;
            OrderItems = new ObservableCollection<OrderItem>(Order.OrderProducts);
        }

        private void LoadOrderData()
        {
            OrderIdEntry.Text = Order.Id.ToString();
            DateCreatedEntry.Text = Order.DateCreated.ToString("g");
            SupplierIdEntry.Text = Order.SupplierId.ToString();

            DeliveryStatusPicker.IsEnabled = false;
            DeliveryStatusPicker.ItemsSource = Enum.GetValues(typeof(DeliveryStatus));
            DeliveryStatusPicker.SelectedItem = Order.DeliveryStatus;

            PayButton.IsEnabled = Order.PayStatus != PayStatus.Paid;
            ReceiveButton.IsEnabled = Order.DeliveryStatus != DeliveryStatus.Received;
            PayStatusPicker.IsEnabled = false;
            PayStatusPicker.ItemsSource = Enum.GetValues(typeof(PayStatus));
            PayStatusPicker.SelectedItem = Order.PayStatus;
            
            OrderProductsList.ItemsSource = OrderItems;
        }

        private async void PayButton_Clicked(object sender, EventArgs e)
        {
            await Loading.RunWithLoading(Navigation, async () =>
            {
                try
                {
                    _controller.PayOrder(Order);
                    Order.PayStatus = PayStatus.Paid;
                    PayStatusPicker.SelectedItem = Order.PayStatus;
                    PayButton.IsEnabled = false;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine($"Error while pay order. Caused by: {exception.Message}\n{exception.StackTrace}");
                    await DisplayAlert("Не удалось оплатить заказ.", exception.Message, "OK");
                }
            });
        }

        private async void ReceiveButton_Clicked(object sender, EventArgs e)
        {
            await Loading.RunWithLoading(Navigation, async () =>
            {
                try
                {
                    _controller.ReceiveOrder(Order);
                    Order.DeliveryStatus = DeliveryStatus.Received;
                    DeliveryStatusPicker.SelectedItem = Order.DeliveryStatus;
                    ReceiveButton.IsEnabled = false;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine($"Error while receiving order. Caused by: {exception.Message}\n{exception.StackTrace}");
                    await DisplayAlert("Не удалось получить заказ.", exception.Message, "OK");
                }
            });
            
        }

        private void RefundButton_Clicked(object sender, EventArgs e)
        {
            _controller.RefundOrder(Order);
            Order.PayStatus = PayStatus.Refund;
            Order.DeliveryStatus = DeliveryStatus.Refund;
            PayStatusPicker.SelectedItem = Order.PayStatus;
            DeliveryStatusPicker.SelectedItem = Order.DeliveryStatus;
        }
    }
}