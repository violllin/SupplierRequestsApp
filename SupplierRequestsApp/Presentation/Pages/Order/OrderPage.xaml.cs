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

            DeliveryStatusPicker.ItemsSource = Enum.GetValues(typeof(DeliveryStatus));
            PayStatusPicker.ItemsSource = Enum.GetValues(typeof(PayStatus));
            
            PayStatusPicker.IsEnabled = false;
            DeliveryStatusPicker.IsEnabled = false;
            
            SetPayStatus(Order.PayStatus, Order.PayStatus != PayStatus.Paid);
            SetDeliveryStatus(Order.DeliveryStatus, Order.DeliveryStatus != DeliveryStatus.Received);

            OrderProductsList.ItemsSource = OrderItems;
        }
        
        private void SetPayStatus(PayStatus payStatus, bool isButtonEnabled = true)
        {
            Order.PayStatus = payStatus;
            PayStatusPicker.SelectedItem = Order.PayStatus;
            PayButton.IsEnabled = isButtonEnabled;
        }

        private void SetDeliveryStatus(DeliveryStatus deliveryStatus, bool isButtonEnabled = true)
        {
            Order.DeliveryStatus = deliveryStatus;
            DeliveryStatusPicker.SelectedItem = Order.DeliveryStatus;
            ReceiveButton.IsEnabled = isButtonEnabled;
        }

        private async void PayButton_Clicked(object sender, EventArgs e)
        {
            await Loading.RunWithLoading(Navigation, async () =>
            {
                try
                {
                    _controller.PayOrder(Order);
                    SetPayStatus(PayStatus.Paid, false);
                    SetDeliveryStatus(DeliveryStatus.InDelivery);
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
                    SetDeliveryStatus(DeliveryStatus.Received, false);
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
            SetPayStatus(PayStatus.Refund);
            SetDeliveryStatus(DeliveryStatus.Refund);
        }
    }
}