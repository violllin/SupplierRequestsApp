using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;
using SupplierRequestsApp.Presentation.Controllers;

namespace SupplierRequestsApp.Presentation.Pages.Storage;

public partial class EditStoragePage : ContentPage
{
    private readonly StoragePageController _controller;
    private readonly Domain.Models.Storage _storage;
    private readonly IStorage<Domain.Models.Product.Product> _productStorageService = new LocalStorageService<Domain.Models.Product.Product>();
    private readonly ObservableCollection<StoredProduct> _selectedProducts = new();
    private List<Domain.Models.Product.Product> _availableProducts;
    private StoredProduct? _currentlyEditingProduct;

    public EditStoragePage(StoragePageController controller, Domain.Models.Storage storage)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        InitializeComponent();
        StorageIdEntry.Text = _storage.StorageId.ToString();
        foreach (var storedProd in _storage.Products)
        {
            _selectedProducts.Add(new StoredProduct
            {
                ProductId = storedProd.ProductId,
                ProductName = storedProd.ProductName,
                Quantity = storedProd.Quantity
            });
        }
        SelectedProductsCollectionView.ItemsSource = _selectedProducts;
        _availableProducts = _productStorageService.LoadEntities(typeof(Domain.Models.Product.Product)).ToList();
        foreach (var sp in _selectedProducts)
        {
            var prod = _availableProducts.FirstOrDefault(p => p.Id == sp.ProductId);
            if (prod != null)
                _availableProducts.Remove(prod);
        }
        ProductPicker.ItemsSource = _availableProducts;
        ProductPicker.ItemDisplayBinding = new Binding("Name");
    }

    private void OnAddProductClicked(object sender, EventArgs e)
    {
        if (ProductPicker.SelectedItem is not Domain.Models.Product.Product selectedProduct)
        {
            DisplayAlert("Ошибка", "Выберите продукт", "ОК");
            return;
        }
        if (!int.TryParse(QuantityEntry.Text, out int quantity) || quantity <= 0)
        {
            DisplayAlert("Ошибка", "Введите корректное количество", "ОК");
            return;
        }
        AddProductButton.Text = "Добавить продукт";
        if (_currentlyEditingProduct != null)
        {
            _currentlyEditingProduct.Quantity = quantity;
            SelectedProductsCollectionView.ItemsSource = null;
            SelectedProductsCollectionView.ItemsSource = _selectedProducts;
            _currentlyEditingProduct = null;
            ProductPicker.SelectedItem = null;
            QuantityEntry.Text = string.Empty;
            return;
        }
        var storedProduct = new StoredProduct
        {
            ProductId = selectedProduct.Id,
            ProductName = selectedProduct.Name,
            Quantity = quantity
        };
        _selectedProducts.Add(storedProduct);
        _availableProducts.Remove(selectedProduct);
        ProductPicker.ItemsSource = null;
        ProductPicker.ItemsSource = _availableProducts;
        ProductPicker.SelectedItem = null;
        QuantityEntry.Text = string.Empty;
    }

    private void OnEditProductClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is StoredProduct selected)
        {
            _currentlyEditingProduct = selected;
            var product = _productStorageService.LoadEntities(typeof(Domain.Models.Product.Product))
                            .FirstOrDefault(p => ((Domain.Models.Product.Product)p).Id == selected.ProductId)
                            as Domain.Models.Product.Product;
            if (product == null)
                return;
            ProductPicker.SelectedItem = product;
            QuantityEntry.Text = selected.Quantity.ToString();
            AddProductButton.Text = "Сохранить";
        }
    }

    private void OnRemoveProductClicked(object sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: StoredProduct selected })
            return;
        if (_productStorageService.LoadEntities(typeof(Domain.Models.Product.Product))
                .FirstOrDefault(p => ((Domain.Models.Product.Product)p).Id == selected.ProductId) is { } product)
        {
            _availableProducts.Add(product);
            ProductPicker.ItemsSource = null;
            ProductPicker.ItemsSource = _availableProducts;
        }
        _selectedProducts.Remove(selected);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            _storage.Products = _selectedProducts.ToList();
            _controller.Service.EditItem(_storage);
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