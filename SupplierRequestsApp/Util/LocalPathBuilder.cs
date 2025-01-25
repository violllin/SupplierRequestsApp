using System.Text;
using Microsoft.Extensions.Primitives;
using SupplierRequestsApp.Domain.Models;
using SupplierRequestsApp.Domain.Models.Product;
using Exception = ABI.System.Exception;

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
            not null when type == typeof(Product) => Config.ProductsStoragePath,
            _ => throw new ArgumentException($"No path to folder for type {type}")
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
                    Filename = storage.StorageId.ToString();
                break;
            case Order order:
                sb.Append(Config.OrdersStoragePath);
                if (withId)
                    Filename = order.Id.ToString();
                break;
            case Product product:
                sb.Append(Config.ProductsStoragePath);
                if (withId)
                    Filename = product.Id.ToString();
                break;
            default:
                throw new ArgumentException($"No path for object with type {item.GetType()}");
        }
        Directory = sb.ToString();
    }
}