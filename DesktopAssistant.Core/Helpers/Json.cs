using Newtonsoft.Json;

namespace DesktopAssistant.Core.Helpers;

/// <summary>
/// Jsonの読み書きを行うヘルパー
/// </summary>
public static class Json
{
    public static async Task<T> ToObjectAsync<T>(string value)
    {
        return await Task.Run<T>(() =>
        {
            return JsonConvert.DeserializeObject<T>(value);
        });
    }

    public static async Task<string> StringifyAsync(object value)
    {
        return await Task.Run<string>(() =>
        {
            return JsonConvert.SerializeObject(value);
        });
    }
}
