using System.Diagnostics;
using SupplierRequestsApp.Presentation.Controllers;
using Microsoft.Maui.Controls;

namespace SupplierRequestsApp.Presentation.Pages.Supplier
{
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
                await Navigation.PushModalAsync(new EditSupplierComponent(_controller));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while open adding new supplier form. Caused by: {ex.Message}\n{ex.StackTrace}");
                await DisplayAlert("Не удалось открыть форму создания поставщика", ex.Message, "ОК");
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var supplier = (Domain.Models.Supplier?)button.CommandParameter;
            if (supplier == null) return;
            try
            {
                await Navigation.PushModalAsync(new EditSupplierComponent(_controller, supplier));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while editing supplier. Caused by: {ex.Message}\n{ex.StackTrace}");
                await DisplayAlert("Не удалось открыть форму редактирования поставщика", ex.Message, "ОК");
            }
        }

        private async void OnRemoveClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var supplier = (Domain.Models.Supplier?)button.CommandParameter;
            if (supplier == null) return;
            bool isConfirmed = await DisplayAlert("Подтвердить удаление",
                $"Вы уверены, что хотите удалить поставщика с ID: {supplier.Id}?",
                "Да", "Нет");
            if (!isConfirmed) return;
            try
            {
                _controller.DropItem(supplier);
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error while drop supplier. Caused by: {exception.Message}\n{exception.StackTrace}");
                await DisplayAlert("Не удалось удалить поставщика", exception.Message, "ОК");
            }
        }
    }
}
