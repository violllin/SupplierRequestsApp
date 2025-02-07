using System.Diagnostics;
using SupplierRequestsApp.Presentation.Controllers;

namespace SupplierRequestsApp.Presentation.Pages.Product;

public partial class ProductsPage : ContentPage
{
    private readonly ProductsPageController _controller = new();

    public ProductsPage()
    {
        InitializeComponent();
        BindingContext = _controller;
    }

    private async void OnAddNewProductClicked(object sender, EventArgs e)
    {
        try
        {
            _controller.AddItem(new Domain.Models.Product(Guid.NewGuid(), "Новый товар", new List<Guid>(), Guid.NewGuid()));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while save new product. Caused by: {ex.Message}");
            await DisplayAlert("Ошибка!", "Не удалось создать продукт.", "ОК");
        }
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var product = (Domain.Models.Product?) button.CommandParameter;
        bool isConfirmed = await DisplayAlert("Подтвердить удаление", 
            $"Вы уверены что хотите удалить продукт с ID: {product.Id}?", 
            "Да", "Нет");
        button.IsEnabled = !isConfirmed;
        if (!isConfirmed) return;
        try
        {
            _controller.DropItem(product);
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Error while drop product. Caused by: {e}");
            await DisplayAlert("Ошибка!", "Не удалось удалить продукт.", "OK");
        }
        finally
        {
            button.IsEnabled = true;
        }
    }
}