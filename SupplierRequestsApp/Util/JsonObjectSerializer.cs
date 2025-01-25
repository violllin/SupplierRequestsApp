using System.Text.Json;

namespace SupplierRequestsApp.Util;

public class JsonObjectSerializer 
{
    public T? Deserialize<T>(string data)
    {
        return JsonSerializer.Deserialize<T>(data, Options);
    }

    public string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, Options);
    }

    private static readonly JsonSerializerOptions Options = new()
    {
        IncludeFields = true,
    };
}