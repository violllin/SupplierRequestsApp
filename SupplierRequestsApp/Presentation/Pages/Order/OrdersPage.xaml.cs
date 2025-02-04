using System.Diagnostics;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Models.Product;
using SupplierRequestsApp.Presentation.Controllers;

namespace SupplierRequestsApp.Presentation.Pages.Order;

public partial class OrdersPage : ContentPage
{
    private readonly OrdersPageController _controller = new();

    public OrdersPage()
    {
        InitializeComponent();
        BindingContext = _controller;
    }

    private async void OnAddNewOrderClicked(object sender, EventArgs e)
    {
        try
        {
            _controller.AddItem(new Domain.Models.Order(Guid.NewGuid(), DateTime.Now, Guid.NewGuid(), new List<OrderItem>(), DeliveryStatus.PAYMENT_WAIT, OrderPayStatus.NOT_PAID));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while adding new order. Caused by: {ex.Message}");
            await DisplayAlert("Ошибка!", "Не удалось создать заказ.", "ОК");
        }
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var order = (Domain.Models.Order?)button.CommandParameter;
    
        if (order == null) return;

        bool isConfirmed = await DisplayAlert("Подтвердить удаление",
            $"Вы уверены, что хотите удалить заказ с ID: {order.Id}?", 
            "Да", "Нет");

        if (!isConfirmed) return;

        try
        {
            _controller.DropItem(order);
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Error while drop order. Caused by: {exception.Message}");
            await DisplayAlert("Ошибка!", "Не удалось удалить заказ.", "ОК");
        }
    }

}