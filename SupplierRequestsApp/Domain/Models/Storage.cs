using SupplierRequestsApp.Data;

namespace SupplierRequestsApp.Domain.Models;

public class Storage
{
    private Guid _storageId;
    private List<Shelf> _shelves;

    public Storage(Guid storageId, List<Shelf>? shelves = null)
    {
        _storageId = storageId;
        _shelves = shelves ?? new List<Shelf>();
    }

    public Guid StorageId
    {
        get => _storageId;
        set => _storageId = Validator.RequireGuid(value);
    }

    public List<Shelf> Shelves
    {
        get => _shelves;
        set => _shelves = value;
    }

    public void AddShelf(Shelf shelf)
    {
        _shelves.Add(shelf);
    }

    public void RemoveShelf(Guid shelfId)
    {
        var shelf = _shelves.FirstOrDefault(s => s.Id == shelfId);
        if (shelf == null)
            throw new InvalidOperationException("Полка не найдена.");

        if (shelf.Slots.Values.Any(p => p != null))
            throw new InvalidOperationException("Нельзя удалить полку, пока на ней есть товары.");

        _shelves.Remove(shelf);
    }

    public void StoreProduct(Guid productId)
    {
        var shelf = _shelves.FirstOrDefault(s => s.CanStore());
        if (shelf == null)
            throw new InvalidOperationException("Нет свободных полок для хранения.");

        shelf.StoreProduct(productId);
    }

    public void RemoveProduct(Guid productId)
    {
        foreach (var shelf in _shelves.Where(shelf => shelf.Slots.ContainsValue(productId)))
        {
            shelf.RemoveProduct(productId);
            return;
        }

        throw new InvalidOperationException("Товар не найден на складе.");
    }
}


public class StoredProduct
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}