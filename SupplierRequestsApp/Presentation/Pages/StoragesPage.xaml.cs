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
            await DisplayAlert("Storage Added", "New storage has been added.", "OK");
        }
        catch (Exception exception)
        {
            await DisplayAlert("Storage Not Added", exception.Message, "Pizdec");
        }
        finally
        {
            _controller.UpdateTable();
        }
    }
    
    private async void OnEditClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var storage = (Storage?) button.CommandParameter;
        await DisplayAlert("Edit Storage", $"Edit storage with ID: {storage.StorageId}", "OK");
    }

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var storage = (Storage?) button.CommandParameter;
        bool isConfirmed = await DisplayAlert("Confirm Deletion", 
            $"Are you sure you want to delete storage with ID: {storage.StorageId}?", 
            "Yes", "No");

        if (!isConfirmed) return;
        try
        {
            _controller.Service.DropItem(storage);
            await DisplayAlert("Success!", "Storage removed.", "OK");
        }
        catch (Exception exception)
        {
            await DisplayAlert("Fail!", exception.Message, "OK");
        }
        finally
        {
            _controller.UpdateTable();
        }
    }
}