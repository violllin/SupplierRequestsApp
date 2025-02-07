using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Presentation.Controllers;
using Microsoft.Maui.Controls;
using SupplierRequestsApp.Util;

namespace SupplierRequestsApp.Presentation.Pages.Storage;

public class ShelfDisplay
{
    public int Index { get; set; }
    public Shelf Shelf { get; set; }
}

public partial class EditStoragePage : ContentPage
{
    private readonly StoragePageController _controller;
    private readonly Domain.Models.Storage _storage;
    private ObservableCollection<ShelfDisplay> _shelfDisplayList = new ObservableCollection<ShelfDisplay>();

    public EditStoragePage(StoragePageController controller, Domain.Models.Storage storage)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        InitializeComponent();
        StorageIdEntry.Text = _storage.StorageId.ToString();
        UpdateShelfDisplayList();
        ShelvesCollectionView.ItemsSource = _shelfDisplayList;
    }

    private void UpdateShelfDisplayList()
    {
        _shelfDisplayList.Clear();
        for (int i = 0; i < _storage.Shelves.Count; i++)
        {
            _shelfDisplayList.Add(new ShelfDisplay { Index = i + 1, Shelf = _storage.Shelves[i] });
        }
    }

    private async void OnAddShelfClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(ShelfCapacityEntry.Text, out int maxCapacity) || maxCapacity <= 0)
        {
            await DisplayAlert("Ошибка", "Введите корректную вместимость", "ОК");
            return;
        }
        
        await Loading.RunWithLoading(Navigation, () =>
        {
            var newShelf = new Shelf(Guid.NewGuid(), maxCapacity);
            _storage.Shelves.Add(newShelf);
            UpdateShelfDisplayList();
            ShelfCapacityEntry.Text = string.Empty;
            return Task.CompletedTask;
        });
    }


    private void OnRemoveShelfClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Shelf shelf)
        {
            if (shelf.Slots.Values.Any(p => p != null))
            {
                DisplayAlert("Ошибка", "Нельзя удалить полку, пока на ней есть товары.", "ОК");
                return;
            }
            _storage.Shelves.Remove(shelf);
            UpdateShelfDisplayList();
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            await Loading.RunWithLoading(Navigation, () =>
            {
                _controller.Service.EditItem(_storage);
                return Task.CompletedTask;
            });
            await DisplayAlert("Успешно!", "Склад обновлен", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error while updating storage. Caused by: " + ex.Message);
            await DisplayAlert("Ошибка!", "Не удалось обновить данные склада.", "ОК");
        }
        finally
        {
            _controller.UpdateTable();
        }
    }
}
