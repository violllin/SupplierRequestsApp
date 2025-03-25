using System.Diagnostics;
using Windows.Devices.PointOfService;
using SupplierRequestsApp.Data;
using SupplierRequestsApp.Util;

namespace SupplierRequestsApp.Domain.Models;

public class Shelf
{
    private Guid _id;
    private int _maxCapacity;
    private Dictionary<int, Guid?> _slots = [];
    private Guid _storageId;

    public Shelf(Guid id, int maxCapacity, Guid storageId)
    {
        Id= id;
        MaxCapacity = Validator.RequireGreaterThan(maxCapacity, 0);
        StorageId = storageId;
        FillSlots();
    }

    private void FillSlots()
    {
        for (var i = _slots.Count; i < _maxCapacity; i++)
        {
            _slots[i] = null;
        }
    }
    
    private void CutSlots()
    {
        for (var i = _slots.Count - 1; i >= _maxCapacity; i--)
        {
            _slots.Remove(i);
        }
    }

    public Task SetMaxCapacity(int capacity)
    {
        if (_maxCapacity >= capacity)
        {
            MaxCapacity = capacity;
            CutSlots();
        }
        else
        {
            MaxCapacity = capacity;
            FillSlots();
        }

        return Task.CompletedTask;
    }
    
    public Guid Id
    {
        get => _id;
        set => _id = Validator.RequireGuid(value);
    }

    public int MaxCapacity
    {
        get => _maxCapacity;
        set
        {
            if (value < MaxCapacity - FreeSlots)
                throw new InvalidCapacityValueException("Нельзя установить размер полки меньше, чем в ней находится товаров.");
            _maxCapacity = Validator.RequireGreaterThan(value, 0);
        }
    }

    public Dictionary<int, Guid?> Slots
    {
        get => _slots;
        set => _slots = value;
    }

    public int FreeSlots => _slots.Count(slot => slot.Value == null);

    public Guid StorageId
    {
        get => _storageId;
        set => _storageId = value;
    }

    public bool CanStore() => FreeSlots > 0;
    
    public bool CanStore(int size) => FreeSlots >= size;

    public void StoreProduct(Guid productId)
    {
        var slot = _slots.FirstOrDefault(slot => slot.Value == null);
        if (!CanStore()) throw new NoFreeSpaceForItemException("На полке нет свободных ячеек.");
        _slots[slot.Key] = productId;
    }

    public void RemoveProduct(Guid productId)
    {
        var found = false;

        foreach (var slotKey in _slots.Keys.Where(k => _slots[k] == productId).ToList())
        {
            _slots[slotKey] = null;
            found = true;
        }

        if (!found)
        {
            Debug.WriteLine($"Такого товара нет на этой полке. ShelfId: {_id}, ProductId: {productId}");
        }
    }
}