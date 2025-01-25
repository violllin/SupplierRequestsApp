using System.Text;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Models.Product;

namespace SupplierRequestsApp.Util;

public static class LocalPathBuilder
{
    public static string BuildPath(object item)
    {
        var sb = new StringBuilder(GetBaseDir());
        var path = item switch
        {
            Supplier supplier => sb.Append(Config.SuppliersStoragePath).Append(supplier.Id),
            Storage storage => sb.Append(Config.StoragesStoragePath).Append(storage.StorageId),
            Order order => sb.Append(Config.OrdersStoragePath).Append(order.Id),
            Product product => sb.Append(Config.ProductsStoragePath).Append(product.Id),
            _ => throw new ArgumentException($"No path for object with type {item.GetType()}")
        };
        return path.Append(".json").ToString();
    }
    
    public static string BuildPath(object item, string fileName)
    {
        var sb = new StringBuilder(GetBaseDir());
        var path = item switch
        {
            Supplier supplier => sb.Append(Config.SuppliersStoragePath),
            Storage storage => sb.Append(Config.StoragesStoragePath),
            Order order => sb.Append(Config.OrdersStoragePath),
            Product product => sb.Append(Config.ProductsStoragePath),
            _ => throw new ArgumentException($"No path for object with type {item.GetType()}")
        };
        return path.Append(fileName).Append(".json").ToString();
    }

    public static string BuildPath(Type type, string fileName)
    {
        var sb = new StringBuilder(GetBaseDir());
        var path = type switch
        {
            not null when type == typeof(Supplier) => sb.Append(Config.SuppliersStoragePath),
            not null when type == typeof(Storage) => sb.Append(Config.StoragesStoragePath),
            not null when type == typeof(Order) => sb.Append(Config.OrdersStoragePath),
            not null when type == typeof(Product) => sb.Append(Config.ProductsStoragePath),
            _ => throw new ArgumentException($"No path for type {type}")
        };
        return path.Append(fileName).Append(".json").ToString();
    }
    public static string BuildFolderPath(Type type)
    {
        var sb = new StringBuilder(GetBaseDir());
        var path = type switch
        {
            not null when type == typeof(Supplier) => sb.Append(Config.SuppliersStoragePath),
            not null when type == typeof(Storage) => sb.Append(Config.StoragesStoragePath),
            not null when type == typeof(Order) => sb.Append(Config.OrdersStoragePath),
            not null when type == typeof(Product) => sb.Append(Config.ProductsStoragePath),
            _ => throw new ArgumentException($"No path to folder for type {type}")
        };
        return path.ToString();
    }

    private static string GetBaseDir()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }
}