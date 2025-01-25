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
        var localPathDirectory = LocalPathBuilder.BuildFolderPath(type);
        if (!Directory.Exists(localPathDirectory.Directory)) return entities;
        var files = Directory.GetFiles(localPathDirectory.Directory, "*.json");
        entities.AddRange(files.Select(File.ReadAllText).Select(jsonString => _serializer.Deserialize<T>(jsonString)).OfType<T>());
        return entities;
    }

    public T? LoadEntity(Type type, string fileName)
    {
        var localPath = LocalPathBuilder.BuildPath(type, fileName);
        if (!File.Exists(localPath.AbsolutePath)) throw new FileNotFoundException($"{localPath} not found");
        var jsonString = File.ReadAllText(localPath.AbsolutePath);
        return _serializer.Deserialize<T>(jsonString);
    }

    public void SaveEntity(T entity)
    {
        var localPath = LocalPathBuilder.BuildPath(entity);
        try
        {
            Debug.WriteLine(localPath.Directory);
            if (!Directory.Exists(localPath.Directory))
            {
                Directory.CreateDirectory(localPath.Directory);
            }
            var jsonString = _serializer.Serialize(entity);
            File.WriteAllText(localPath.AbsolutePath, jsonString);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Cannot save entity {entity}. Error in: {e}");
        }
        finally
        {
            if (File.Exists(localPath.AbsolutePath))
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
            if (File.Exists(path.AbsolutePath))
            {
                var jsonString = _serializer.Serialize(updatedEntity);
                File.WriteAllText(path.AbsolutePath, jsonString);
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
            File.Delete(path.AbsolutePath);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Cannot drop entity {entity}. Error in: {e}");
        }
        finally
        {
            if (!File.Exists(path.AbsolutePath))
            {
                Debug.WriteLine($"Entity {entity} dropped successfully.");
            }
        }
    }

}