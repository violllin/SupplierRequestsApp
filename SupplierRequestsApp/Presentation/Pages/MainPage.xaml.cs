using System.Diagnostics;
using SupplierRequestsApp.Presentation.Pages.Order;
using SupplierRequestsApp.Presentation.Pages.Storage;
using SupplierRequestsApp.Presentation.Pages.Supplier;

namespace SupplierRequestsApp.Presentation.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnStoragesPageClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new StoragesPage());
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Can't open storages page. Caused by: {exception.Message}");
            await DisplayAlert("Ошибка!", "Произошла ошибка при попытке открыть страницу.", "ОК");
        }
    }


    private async void OnOrdersPageClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new OrdersPage());
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Can't open storages page. Caused by: {exception.Message}");
            await DisplayAlert("Ошибка!", "Произошла ошибка при попытке открыть страницу.", "ОК");
        }
    }

    private async void OnSupplierPageClicked(object? sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new SuppliersPage());
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Can't open suppliers page. Caused by: {exception.Message}");
            await DisplayAlert("Ошибка!", "Произошла ошибка при попытке открыть страницу.", "ОК");
        }
    }
}