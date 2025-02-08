using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Presentation.Controllers;

namespace SupplierRequestsApp.Presentation.Pages.Storage;

public partial class StoragesPage : ContentPage
{

    private readonly StoragePageController _controller = new();

    public StoragesPage()
    {
        InitializeComponent();
        BindingContext = _controller;
    }

    private async void OnAddNewStorageClicked(object sender, EventArgs e)
    {
        try
        {
            _controller.AddItem(new Domain.Models.Storage(Guid.NewGuid(), []));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while save new storage. Caused by: {ex.Message}\n{ex.StackTrace}");
            await DisplayAlert("Не удалось создать склад", ex.Message, "ОК");
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var storage = (Domain.Models.Storage?) button.CommandParameter;
        try
        {
            await Navigation.PushAsync(new  EditStoragePage(_controller, storage));
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Error while open EditStoragePage with storage argument. Caused by: {exception.Message}\n{exception.StackTrace}");
            await DisplayAlert("Не удалось открыть форму редактирования склада", exception.Message, "ОК");
        }
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var storage = (Domain.Models.Storage?) button.CommandParameter;
        bool isConfirmed = await DisplayAlert("Подтвердить удаление", 
            $"Вы уверены что хотите удалить склад с ID: {storage.Id}?", 
            "Да", "Нет");
        button.IsEnabled = !isConfirmed;
        if (!isConfirmed) return;
        try
        {
            _controller.DropItem(storage);
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Error while drop storage. Caused by: {exception.Message}\n{exception.StackTrace}");
            await DisplayAlert("Не удалось удалить склад", exception.Message, "OK");
        }
        finally
        {
            button.IsEnabled = true;
        }
    }
    
    private async Task<Dictionary<Guid, int>> DisplayProductSelectionDialog(ObservableCollection<Domain.Models.Product> allProducts)
    {
        var selectedProducts = new Dictionary<Guid, int>();
        var productSelections = new Dictionary<Domain.Models.Product, bool>();

        foreach (var product in allProducts)
            productSelections[product] = false;

        string result = await DisplayActionSheet("Выберите продукты:", "Отмена", "Добавить", allProducts.Select(p => p.Name).ToArray());

        if (result != "Добавить" && result != "Отмена")
        {
            var selectedProduct = allProducts.FirstOrDefault(p => p.Name == result);
            if (selectedProduct != null)
                selectedProducts.Add(selectedProduct.Id, new Random().Next(1, 100));
        }

        return selectedProducts;
    }
}