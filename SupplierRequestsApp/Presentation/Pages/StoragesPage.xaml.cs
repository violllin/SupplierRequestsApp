using System.Diagnostics;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Presentation.Controllers;

namespace SupplierRequestsApp.Presentation.Pages;

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
        var newStorage = new Storage(Guid.NewGuid(), new Dictionary<int, int> { { 1, 100 }, { 2, 200 } });
        try
        {
            _controller.Service.AddItem(newStorage);
            await DisplayAlert("Успешно!", "Склад добавлен", "OK");
        }
        catch (Exception exception)
        {
            await DisplayAlert("Ошибка!", "Не удалось добавить склад", "ОК");
            Debug.WriteLine($"Error while add storage. Caused by: {exception}");
        }
        finally
        {
            _controller.UpdateTable();
        }
    }
    
    private async void OnEditClicked(object sender, EventArgs e)
    {
        // var button = (Button)sender;
        // var storage = (Storage?) button.CommandParameter;
        // await DisplayAlert("Edit Storage", $"Edit storage with ID: {storage.StorageId}", "OK");
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var storage = (Storage?) button.CommandParameter;
        bool isConfirmed = await DisplayAlert("Подтвердить удаление", 
            $"Вы уверены что хотите удалить склад с ID: {storage.StorageId}?", 
            "Да", "Нет");

        if (!isConfirmed) return;
        try
        {
            _controller.Service.DropItem(storage);
            await DisplayAlert("Успешно!", "Склад удален.", "OK");
        }
        catch (Exception exception)
        {
            await DisplayAlert("Ошибка!", "Не удалось удалить склад.", "OK");
            Debug.WriteLine($"Error while drop storage. Caused by: {e}");
        }
        finally
        {
            _controller.UpdateTable();
        }
    }
}