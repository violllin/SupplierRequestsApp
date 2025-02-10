using System.Text;
using SupplierRequestsApp.Domain.Models;

namespace SupplierRequestsApp.Util;

public static class LocalPathBuilder
{
    public static LocalPath BuildPath(object item)
    {
        var localPath = new LocalPath(directory: GetBaseDir());
        localPath.ConcreteDirectory(item, true);
        return localPath;
    }

    public static LocalPath BuildPath(object item, string fileName)
    {
        var localPath = new LocalPath(directory: GetBaseDir(), filename: fileName);
        localPath.ConcreteDirectory(item, false);
        return localPath;
    }

    public static LocalPath BuildPath(Type type, string fileName)
    {
        var localPath = new LocalPath(directory: GetBaseDir(), filename: fileName);
        localPath.ConcreteDirectory(type);
        return localPath;
    }

    public static LocalPath BuildFolderPath(Type type)
    {
        var localPath = new LocalPath(directory: GetBaseDir());
        localPath.ConcreteDirectory(type);
        return localPath;
    }

    private static string GetBaseDir()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/SupplierRequestsApp/";
    }
}

public class LocalPath
{
    public string Directory { get; set; }

    public string Filename { get; set; }

    public string Extension { get; set; }

    public string AbsolutePath => Directory + Filename + Extension;

    public LocalPath()
    {
        Directory = "";
        Filename = "";
        Extension = ".json";
    }

    public LocalPath(string directory, string filename, string extension = ".json")
    {
        Directory = directory;
        Filename = filename;
        Extension = extension;
    }

    public LocalPath(string directory, string extension = ".json")
    {
        Directory = directory;
        Filename = "";
        Extension = extension;
    }

    public void ConcreteDirectory(Type type)
    {
        var typeDir = type switch
        {
            not null when type == typeof(Supplier) => Config.SuppliersStoragePath,
            not null when type == typeof(Storage) => Config.StoragesStoragePath,
            not null when type == typeof(Order) => Config.OrdersStoragePath,
            not null when type == typeof(OrderItem) => Config.OrderItemsStoragePath,
            not null when type == typeof(Product) => Config.ProductsStoragePath,
            not null when type == typeof(Shelf) => Config.ShelfStoragePath,
            _ => throw new UndefinedFolderTypeException($"Не найден путь до хранилища для объекта с типом: {type}")
        };
        Directory += typeDir;
    }

    public void ConcreteDirectory(object item, bool withId)
    {
        var sb = new StringBuilder(Directory);
        switch (item)
        {
            case Supplier supplier:
                sb.Append(Config.SuppliersStoragePath);
                if (withId)
                    Filename = supplier.Id.ToString();
                break;
            case Storage storage:
                sb.Append(Config.StoragesStoragePath);
                if (withId)
                    Filename = storage.Id.ToString();
                break;
            case Order order:
                sb.Append(Config.OrdersStoragePath);
                if (withId)
                    Filename = order.Id.ToString();
                break;
            case OrderItem orderItem:
                sb.Append(Config.OrderItemsStoragePath);
                if (withId)
                    Filename = orderItem.Id.ToString();
                break;
            case Product product:
                sb.Append(Config.ProductsStoragePath);
                if (withId)
                    Filename = product.Id.ToString();
                break;
            case Shelf shelf:
                sb.Append(Config.ShelfStoragePath);
                if (withId)
                    Filename = shelf.Id.ToString();
                break;
            default:
                throw new UndefinedFolderTypeException($"Не найден путь до хранилища для объекта с типом: {item.GetType()}");
        }
        Directory = sb.ToString();
    }
}