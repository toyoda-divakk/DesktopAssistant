using System.Text.Json;
using Windows.Storage;
using Windows.Storage.Streams;

namespace DesktopAssistant.Helpers;

// ローカルおよびローミングアプリのデータを保存および取得するには、以下の拡張メソッドを使用します。
// https://docs.microsoft.com/windows/apps/design/app-settings/store-and-retrieve-app-data
public static class SettingsStorageExtensions
{
    private const string FileExtension = ".json";

#pragma warning disable CA1416
    public static bool IsRoamingStorageAvailable(this ApplicationData appData) => appData.RoamingStorageQuota == 0;

    public static async Task SaveAsync<T>(this StorageFolder folder, string name, T content)
    {
        var file = await folder.CreateFileAsync(GetFileName(name), CreationCollisionOption.ReplaceExisting);
        var fileContent = JsonSerializer.Serialize(content);

        await FileIO.WriteTextAsync(file, fileContent);
    }

    public static async Task<T?> ReadAsync<T>(this StorageFolder folder, string name)
    {
        if (!File.Exists(Path.Combine(folder.Path, GetFileName(name))))
        {
            return default;
        }

        var file = await folder.GetFileAsync($"{name}.json");
        var fileContent = await FileIO.ReadTextAsync(file);

        return JsonSerializer.Deserialize<T>(fileContent);
    }

    public static void Save<T>(this ApplicationDataContainer settings, string key, T value)
    {
        settings.SaveString(key, JsonSerializer.Serialize(value));
    }

    public static void SaveString(this ApplicationDataContainer settings, string key, string value)
    {
        settings.Values[key] = value;
    }

    public static T? Read<T>(this ApplicationDataContainer settings, string key)
    {
        object? obj;

        if (settings.Values.TryGetValue(key, out obj))
        {
            return JsonSerializer.Deserialize<T>((string)obj);
        }

        return default;
    }

    public static async Task<StorageFile> SaveFileAsync(this StorageFolder folder, byte[] content, string fileName, CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
    {
        if (content == null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("File name is null or empty. Specify a valid file name", nameof(fileName));
        }

        var storageFile = await folder.CreateFileAsync(fileName, options);
        await FileIO.WriteBytesAsync(storageFile, content);
        return storageFile;
    }

    public static async Task<byte[]?> ReadFileAsync(this StorageFolder folder, string fileName)
    {
        var item = await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false);

        if ((item != null) && item.IsOfType(StorageItemTypes.File))
        {
            var storageFile = await folder.GetFileAsync(fileName);
            var content = await storageFile.ReadBytesAsync();
            return content;
        }

        return null;
    }

    public static async Task<byte[]?> ReadBytesAsync(this StorageFile file)
    {
        if (file != null)
        {
            using IRandomAccessStream stream = await file.OpenReadAsync();
            using var reader = new DataReader(stream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)stream.Size);
            var bytes = new byte[stream.Size];
            reader.ReadBytes(bytes);
            return bytes;
        }

        return null;
    }

    private static string GetFileName(string name)
    {
        return string.Concat(name, FileExtension);
    }
}

#pragma warning restore CA1416