using SupplierRequestsApp.Data;

namespace SupplierRequestsApp.Domain.Models;

public class Shelf
{
    private Guid _id;
    private int _maxCapacity;
    private Dictionary<int, Guid?> _slots;

    public Shelf(Guid id, int maxCapacity, Dictionary<int, Guid?>? slots = null)
    {
        _id = id;
        _maxCapacity = Validator.RequireGreaterThan(maxCapacity, 0);
        _slots = slots ?? new Dictionary<int, Guid?>();

        for (int i = _slots.Count; i < _maxCapacity; i++)
        {
            _slots[i] = null;
        }
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
            if (value < _slots.Count)
                throw new ArgumentOutOfRangeException("Нельзя уменьшить MaxCapacity меньше текущего количества слотов.");
            _maxCapacity = Validator.RequireGreaterThan(value, 0);
        }
    }

    public Dictionary<int, Guid?> Slots => _slots;

    public int FreeSlots => _slots.Count(slot => slot.Value == null);

    public bool CanStore() => FreeSlots > 0;

    public void StoreProduct(Guid productId)
    {
        foreach (var slot in _slots.Where(slot => slot.Value == null))
        {
            _slots[slot.Key] = productId;
            return;
        }

        throw new InvalidOperationException("Нет свободных слотов.");
    }

    public void RemoveProduct(Guid productId)
    {
        foreach (var slot in _slots.Where(slot => slot.Value == productId))
        {
            _slots[slot.Key] = null;
            return;
        }

        throw new InvalidOperationException("Такого товара нет на этой полке.");
    }
}