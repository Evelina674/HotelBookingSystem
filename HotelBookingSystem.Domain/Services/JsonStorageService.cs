using System.Text.Json;

namespace HotelBookingSystem.Domain.Services;

public class JsonStorageService<T>
{
    public void Save(string filePath, T data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(filePath, json);
    }

    public T? Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return default;
        }

        var json = File.ReadAllText(filePath);

        return JsonSerializer.Deserialize<T>(json);
    }
}