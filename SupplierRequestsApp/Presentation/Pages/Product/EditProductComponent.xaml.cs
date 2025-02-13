using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Presentation.Controllers;
using SupplierRequestsApp.Util;
using SupplierRequestsApp.Util.Converters;

namespace SupplierRequestsApp.Presentation.Pages.Product
{
    public partial class EditProductComponent : ContentPage
    {
        private readonly ProductsPageController _controller;
        private Domain.Models.Product? _product;
        public ObservableCollection<Domain.Models.Supplier> SelectedSuppliers { get; set; } = [];

        public EditProductComponent(ProductsPageController controller, Domain.Models.Product? product = null)
        {
            InitializeComponent();
            _controller = controller;
            BindingContext = this;
            LoadPageData(product);
        }
        
        private (List<Domain.Models.Supplier>, List<Shelf>) BindPickers(Domain.Models.Product? product)
        {
            var suppliers = _controller.LoadSuppliers();
            SuppliersPicker.ItemsSource = suppliers;
            SuppliersPicker.ItemDisplayBinding = new Binding("Name");

            var storages = _controller.LoadStorages();
            var shelves = _controller.LoadShelves(storages);
            if (shelves.Count != 0)
            {
                ShelvesPicker.ItemsSource = shelves;
            }

            if (product != null)
            {
                var selShelf = shelves.FirstOrDefault(s => s.Id == product.ShelfId);
                if (selShelf != null)
                    ShelvesPicker.SelectedItem = selShelf;
            }

            return (suppliers, shelves);
        }

        private void BindSelectedSuppliers(List<Domain.Models.Supplier> suppliers, Domain.Models.Product product)
        {
            var selectedSuppliers = suppliers.Where(s =>
                product.SuppliersId.Contains(s.Id));

            foreach (var supplier in selectedSuppliers)
            {
                SelectedSuppliers.Add(supplier);
            }
        }

        private void LoadPageData(Domain.Models.Product? product)
        {
            var (suppliers, shelves) = BindPickers(product);
            if (product == null) return;
            _product = product;
            Title = "Редактирование товара";
            NameEntry.Text = _product.Name;
            BindSelectedSuppliers(suppliers, product);
        }

        private void OnSupplierSelected(object sender, EventArgs e)
        {
            if (SuppliersPicker.SelectedItem is Domain.Models.Supplier selectedSupplier
                && !SelectedSuppliers.Contains(selectedSupplier))
            {
                SelectedSuppliers.Add(selectedSupplier);
            }
            SuppliersPicker.SelectedItem = null;
        }

        private void OnRemoveSupplier(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is Domain.Models.Supplier supplierToRemove)
            {
                SelectedSuppliers.Remove(supplierToRemove);
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text;
            var selectedShelf = ShelvesPicker.SelectedItem as Shelf;

            if (string.IsNullOrWhiteSpace(name) || SelectedSuppliers.Count == 0 || selectedShelf == null)
            {
                await DisplayAlert("Внимание", "Заполните все поля", "ОК");
                return;
            }

            var supplierIds = SelectedSuppliers.Select(s => s.Id).ToList();

            if (_product != null)
            {
                try
                {
                    await Loading.RunWithLoading(Navigation, () =>
                    {
                        _product.PreviosShelfId = _product.ShelfId;
                        _product.PreviosSuppliersId = _product.SuppliersId;
                        _product.Name = name;
                        _product.SuppliersId = supplierIds;
                        _product.ShelfId = selectedShelf.Id;
                        _controller.EditItem(_product);
                        return Task.CompletedTask;
                    });
                    await Navigation.PopAsync();
                }
                catch (Exception exception)
                {
                    Debug.WriteLine($"Error while editing product: {exception.Message}\n{exception.StackTrace}");
                    await DisplayAlert("Ошибка сохранения", exception.Message, "ОК");
                }
            }
            else
            {
                if (_controller.LoadProducts()
                        .Where(product => product.Name == name).ToList().Count != 0)
                {
                    await DisplayAlert("Ошибка сохранения", $"Продукт с названием {name} уже существует", "ОК");
                    return;
                }
                try
                {
                    await Loading.RunWithLoading(Navigation, () =>
                    {
                        var newProduct = new Domain.Models.Product(Guid.NewGuid(), name, supplierIds, selectedShelf.Id,
                            selectedShelf.Id, supplierIds);
                        _controller.AddItem(newProduct);
                        return Task.CompletedTask;
                    });
                    await Navigation.PopAsync();
                }
                catch (Exception exception)
                {
                    Debug.WriteLine($"Cannot add product. Error in: {exception.Message}\n{exception.StackTrace}");
                    await DisplayAlert("Ошибка сохранения", exception.Message, "ОК");
                }
            }
        }
    }
}