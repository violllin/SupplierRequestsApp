using System.Diagnostics;
using SupplierRequestsApp.Presentation.Controllers;

namespace SupplierRequestsApp.Presentation.Pages.Order;

public partial class DeliveryPage : ContentPage
{

    private readonly DeliveryPageController _controller = new();
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
}
