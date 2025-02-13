using System.Collections.ObjectModel;
using System.ComponentModel;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Presentation.Controllers;

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
            DeliveryStatusPicker.SelectedItem = Order.DeliveryStatus;

            PayStatusPicker.ItemsSource = Enum.GetValues(typeof(PayStatus));
            PayStatusPicker.SelectedItem = Order.PayStatus;

            OrderProductsList.ItemsSource = OrderItems;
        }

        private void PayButton_Clicked(object sender, EventArgs e)
        {
            if (sender is not Button { BindingContext: Domain.Models.Order order }) return;
            _controller.PayOrder(order);
            Order.PayStatus = PayStatus.Paid;
            PayStatusPicker.SelectedItem = Order.PayStatus;
        }

        private void ReceiveButton_Clicked(object sender, EventArgs e)
        {
            if (sender is not Button { BindingContext: Domain.Models.Order order }) return;
            _controller.ReceiveOrder(order);
            Order.DeliveryStatus = DeliveryStatus.Received;
            DeliveryStatusPicker.SelectedItem = Order.DeliveryStatus;
        }

        private void RefundButton_Clicked(object sender, EventArgs e)
        {
            if (sender is not Button { BindingContext: Domain.Models.Order order }) return;
            _controller.RefundOrder(order);
            Order.PayStatus = PayStatus.Refund;
            Order.DeliveryStatus = DeliveryStatus.Refund;
            PayStatusPicker.SelectedItem = Order.PayStatus;
            DeliveryStatusPicker.SelectedItem = Order.DeliveryStatus;
        }
    }
}