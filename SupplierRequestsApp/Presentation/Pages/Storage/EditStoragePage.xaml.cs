using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Presentation.Controllers;
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
    private ObservableCollection<ShelfDisplay> _shelfDisplayList = [];

    public EditStoragePage(StoragePageController controller, Domain.Models.Storage storage)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        InitializeComponent();
        StorageIdEntry.Text = _storage.Id.ToString();
        UpdateShelfDisplayList();
        ShelvesCollectionView.ItemsSource = _shelfDisplayList;
    }

    private void UpdateShelfDisplayList()
    {
        _shelfDisplayList.Clear();
        int i = 1;
        foreach (var shelf in _controller.GetShelves(_storage.Id))
        {
            _shelfDisplayList.Add(new ShelfDisplay{Index = i++, Shelf = shelf});
        }
    }

    private async void OnAddShelfClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(ShelfCapacityEntry.Text, out int maxCapacity) || maxCapacity <= 0)
        {
            await DisplayAlert("Внимание", "Введите корректную вместимость", "ОК");
            return;
        }
        
        await Loading.RunWithLoading(Navigation, () =>
        {
            var newShelf = new Shelf(Guid.NewGuid(), maxCapacity, _storage.Id);
            _controller.AddShelf(newShelf);
            _storage.Shelves.Add(newShelf.Id);
            UpdateShelfDisplayList();
            ShelfCapacityEntry.Text = string.Empty;
            return Task.CompletedTask;
        });
    }


    private async void OnRemoveShelfClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Shelf shelf)
        {
            if (shelf.Slots.Values.Any(p => p != null))
            {
                await DisplayAlert("Ошибка", "Нельзя удалить полку, пока на ней есть товары.", "ОК");
                return;
            }
            _storage.Shelves.Remove(shelf.Id);
            _controller.DropShelf(shelf);
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
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error while updating storage. Caused by: " + ex.Message + "\n" + ex.StackTrace);
            await DisplayAlert("Не удалось обновить данные склада", ex.Message, "ОК");
        }
        finally
        {
            _controller.UpdateTable();
        }
    }

    private async void OnEditShelfClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Shelf shelf)
        {
            string result = await DisplayPromptAsync("Редактирование полки",
                "Введите новую вместимость:", 
                "OK", "Отмена", 
                initialValue: shelf.MaxCapacity.ToString(), 
                keyboard: Keyboard.Numeric);

            if (int.TryParse(result, out int newCapacity) && newCapacity > 0)
            {
                try
                {
                    await shelf.SetMaxCapacity(newCapacity);
                    _controller.UpdateShelf(shelf);
                    UpdateShelfDisplayList();
                }
                catch (Exception exception)
                {
                    Debug.WriteLine($"Error while edit shelf. Caused by: {exception.Message}\n{exception.StackTrace}");
                    await DisplayAlert("Не удалось изменить полку", exception.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Введите корректное число!", "OK");
            }
        }
    }

}
