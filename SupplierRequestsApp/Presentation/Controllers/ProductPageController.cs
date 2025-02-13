using System.Collections.ObjectModel;
using System.Diagnostics;
using SupplierRequestsApp.Data.Service;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Service;
using SupplierRequestsApp.Util;

namespace SupplierRequestsApp.Presentation.Controllers;

public class ProductsPageController
{
    private readonly ITableService<Product> _productsTable = new TableService<Product>();
    private readonly IStorage<Supplier> _supplierService = new LocalStorageService<Supplier>();
    private readonly IStorage<Storage> _storageService = new LocalStorageService<Storage>();
    private readonly IStorage<Product> _productService = new LocalStorageService<Product>();
    private readonly IStorage<Shelf> _shelfService = new LocalStorageService<Shelf>();
    public ObservableCollection<Product> Products { get; set; } = [];

    public ProductsPageController()
    {
        UpdateTable();
    }
    
    public List<Product> LoadProducts()
    {
        return _productService.LoadEntities()
            .ToList();
    }
    
    public List<Storage> LoadStorages()
    {
        return _storageService.LoadEntities()
            .ToList();
    }

    public List<Supplier> LoadSuppliers()
    {
        return _supplierService.LoadEntities()
            .ToList();
    }
    
    public List<Shelf> LoadShelves(List<Storage> storages)
    {
        List<Shelf> shelves = [];
        foreach (var storage in storages)
        {
            shelves.AddRange(storage.Shelves.Select(shelfId => _shelfService.LoadEntity(shelfId.ToString())).OfType<Shelf>().Where(shelf => shelf.CanStore()));
        }

        return shelves;
    }

    public void UpdateTable()
    {
        _productsTable.UpdateTable();
        Products.Clear();
        foreach (var product in _productsTable.Data)
        {
            Products.Add(product);
        }
    }

    public void AddItem(Product product)
    {
        foreach (var loadedSupplier in product.SuppliersId.Select(supplier => _supplierService.LoadEntity(supplier.ToString())))
        {
            if (loadedSupplier == null) throw new SupplierNotFoundException("Поставщик с таким ID не найден.");
            loadedSupplier.Products.Add(product.Id);
            _supplierService.UpdateEntity(loadedSupplier);
        }
        _productsTable.AddItem(product);
        Products.Add(product);
        _productsTable.UpdateTable();
    }

    public void DropItem(Product product)
    {
        foreach (var supplier in product.SuppliersId.Select(supplierId => _supplierService.LoadEntity(supplierId.ToString())))
        {
            if (supplier == null) throw new SupplierNotFoundException("Поставщик с таким ID не найден.");
            supplier.Products.Remove(product.Id);
            _supplierService.UpdateEntity(supplier);
        }
        
        try
        {
            var shelf = _shelfService.LoadEntity(product.ShelfId.ToString());
            if (shelf == null) throw new ShelfNotFoundException("Полка с таким ID не найдена.");
            shelf.RemoveProduct(product.Id);
            _shelfService.UpdateEntity(shelf);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error while removing product from shelf: {e.Message}\n{e.StackTrace}");
        }
        _productsTable.DropItem(product);
        Products.Remove(product);
        _productsTable.UpdateTable();
    }

    public void EditItem(Product product)
    {
        var oldShelf = _shelfService.LoadEntity(product.PreviosShelfId.ToString());
        try
        {
            oldShelf?.RemoveProduct(product.Id);
            if (oldShelf != null) _shelfService.UpdateEntity(oldShelf);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error while removing product from shelf: {e.Message}");
        }
        foreach (var supplier in product.PreviosSuppliersId.Select(prevSupplier => _supplierService.LoadEntity(prevSupplier.ToString())))
        {
            supplier?.Products.Remove(product.Id);
            if (supplier != null) _supplierService.UpdateEntity(supplier);
        }

        foreach (var supplier in product.SuppliersId.Select(newSupplier => _supplierService.LoadEntity(newSupplier.ToString())))
        {
            if (supplier == null) throw new SupplierNotFoundException("Поставщик с таким ID не найден.");
            supplier.Products.Add(product.Id);
            _supplierService.UpdateEntity(supplier);
        }
        _productsTable.EditItem(product);
        Products[Products.IndexOf(product)] = product;
        _productsTable.UpdateTable();
    }
    
}