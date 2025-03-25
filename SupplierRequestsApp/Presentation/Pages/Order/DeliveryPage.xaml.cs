using System.Diagnostics;
using SupplierRequestsApp.Presentation.Controllers;

namespace SupplierRequestsApp.Presentation.Pages.Order;

public partial class DeliveryPage : ContentPage
{

    private readonly DeliveryPageController _controller = new();
    private bool _isArchiveShown;
    public DeliveryPage()
    {
        InitializeComponent();
        BindingContext = _controller;
    }
    
    private async void OnViewOrderClicked(object sender, EventArgs e)
    {
        if (sender is not Button { BindingContext: Domain.Models.Order order }) return;
        try
        {
            await Navigation.PushAsync(new OrderPage(order, _controller));
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Error while open Order page. Caused by: {exception.Message}\n{exception.StackTrace}");
            await DisplayAlert("Не удалось открыть страницу заказа.", exception.Message, "OK");
        }
    }

    private void OnShowCompletedOrdersClicked(object? sender, EventArgs e)
    {
        _isArchiveShown = !_isArchiveShown;
        _controller.ForceUpdateTable(_isArchiveShown);
        ShowArchivedOrdersButton.Text = _isArchiveShown ? "Скрыть завершенные заказы" : "Показать завершенные заказы";
    }

    private async void OnDropOrderClicked(object sender, EventArgs e)
    {
        if (sender is not Button { BindingContext: Domain.Models.Order order }) return;
        bool confirm = await DisplayAlert("Подтверждение", "Вы уверены, что хотите удалить этот заказ?", "Да", "Нет");
        if (!confirm) return;

        try
        {
            _controller.DropOrder(order);
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Error while dropping order. Caused by: {exception.Message}\n{exception.StackTrace}");
            await DisplayAlert("Не удалось удалить заказ.", exception.Message, "OK");
        }
    }
}
