using System.Diagnostics;
using SupplierRequestsApp.Presentation.Controllers;

namespace SupplierRequestsApp.Presentation.Pages.Storage;

public partial class EditStoragePage : ContentPage
{
    private readonly StoragePageController _controller;
    private readonly Domain.Models.Storage? _storage;

    public EditStoragePage(StoragePageController controller, Domain.Models.Storage? storage)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        InitializeComponent();
        StorageIdEntry.Text = _storage.StorageId.ToString();
        ProductsEntry.Text = _storage.Products.Count.ToString();
        ProductsEntry.IsEnabled = true;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            _storage!.Products.Add(Guid.NewGuid(), 123); // TODO
            _controller.Service.EditItem(_storage!);
            await DisplayAlert("Успешно!", "Склад обновлен", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error while update storage. Caused by: " + ex.Message);
            await DisplayAlert("Ошибка!", "Не удалось обновить данные склада.", "ОК");
        }
        finally
        {
            _controller.UpdateTable();
        }
    }
}