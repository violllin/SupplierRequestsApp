using System.Diagnostics;
using SupplierRequestsApp.Domain.Service;
using SupplierRequestsApp.Util;

namespace SupplierRequestsApp.Data.Service;

public class LocalStorageService<T> : IStorage<T> where T : class
{
    private readonly JsonObjectSerializer _serializer = new();

    public IEnumerable<T> LoadEntities(Type type)
    {
        List<T> entities = [];
        var dir = LocalPathBuilder.BuildFolderPath(type);
        if (!Directory.Exists(dir)) return entities;
        var files = Directory.GetFiles(dir, "*.json");
        entities.AddRange(files.Select(File.ReadAllText).Select(jsonString => _serializer.Deserialize<T>(jsonString)).OfType<T>());
        return entities;
    }

    public T? LoadEntity(Type type, string fileName)
    {
        var filePath = LocalPathBuilder.BuildPath(type, fileName);
        if (!File.Exists(filePath)) throw new FileNotFoundException($"{filePath} not found");
        var jsonString = File.ReadAllText(filePath);
        return _serializer.Deserialize<T>(jsonString);
    }

    public void SaveEntity(T entity)
    {
        var path = LocalPathBuilder.BuildPath(entity);
        try
        {
            var jsonString = _serializer.Serialize(entity);
            File.WriteAllText(path, jsonString);
            Debug.WriteLine($"Entity {entity} saved successfully.");
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Cannot drop entity {entity}. Error in: {e}");
        }
        finally
        {
            if (File.Exists(path))
            {
                Debug.WriteLine($"Entity {entity} saved successfully.");
            }
        }
    }

    public void UpdateEntity(T updatedEntity)
    {
        var path = LocalPathBuilder.BuildPath(updatedEntity);
        try
        {
            if (File.Exists(path))
            {
                var jsonString = _serializer.Serialize(updatedEntity);
                File.WriteAllText(path, jsonString);
                Debug.WriteLine($"Entity {updatedEntity} updated successfully.");
            }
            else
            {
                SaveEntity(updatedEntity);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Cannot update entity {updatedEntity}. Error in: {e}");
        }
    }

    public void DropEntity(T entity)
    {
        var path = LocalPathBuilder.BuildPath(entity);
        try
        {
            File.Delete(path);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Cannot drop entity {entity}. Error in: {e}");
        }
        finally
        {
            if (!File.Exists(path))
            {
                Debug.WriteLine($"Entity {entity} dropped successfully.");
            }
        }
    }

}