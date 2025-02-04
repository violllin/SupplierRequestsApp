using System.Diagnostics;
using SupplierRequestsApp.Domain.Models.Product;
using SupplierRequestsApp.Presentation.Controllers;

namespace SupplierRequestsApp.Presentation.Pages.Supplier;

public partial class SuppliersPage : ContentPage
{
    private readonly SuppliersPageController _controller = new();

    public SuppliersPage()
    {
        InitializeComponent();
        BindingContext = _controller;
    }

    private async void OnAddNewSupplierClicked(object sender, EventArgs e)
    {
        try
        {
            _controller.AddItem(new Domain.Models.Supplier(Guid.NewGuid(), "Новый поставщик", "Адрес", "Телефон", new List<Product>()));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while adding new supplier. Caused by: {ex.Message}");
            await DisplayAlert("Ошибка!", "Не удалось создать заказчика.", "ОК");
        }
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var order = (Domain.Models.Supplier?)button.CommandParameter;
    
        if (order == null) return;

        bool isConfirmed = await DisplayAlert("Подтвердить удаление",
            $"Вы уверены, что хотите удалить заказчика с ID: {order.Id}?", 
            "Да", "Нет");

        if (!isConfirmed) return;
        try
        {
            _controller.DropItem(order);
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Error while drop supplier. Caused by: {exception.Message}");
            await DisplayAlert("Ошибка!", "Не удалось удалить поставщика.", "ОК");
        }
    }
}