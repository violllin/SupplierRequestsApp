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
            await Navigation.PushAsync(new EditProductComponent(_controller));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while open add new product page. Caused by: {ex.Message}\n{ex.StackTrace}");
            await DisplayAlert("Не удалось открыть форму создания продукта", ex.Message, "ОК");
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var product = (Domain.Models.Product?)button.CommandParameter;
        if (product == null) return;
        try
        {
            await Navigation.PushAsync(new EditProductComponent(_controller, product));
        }
        catch (Exception exception)
        {
            Debug.WriteLine(
                $"Error while open edit product page. Caused by: {exception.Message}\n{exception.StackTrace}");
            await DisplayAlert("Не удалось открыть форму редактирования продукта", exception.Message, "ОК");
        }
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var product = (Domain.Models.Product?)button.CommandParameter;
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
            Debug.WriteLine($"Error while drop product. Caused by: {exception.Message}\n{exception.StackTrace}");
            await DisplayAlert("Не удалось удалить продукт", exception.Message, "OK");
        }
        finally
        {
            button.IsEnabled = true;
        }
    }
}