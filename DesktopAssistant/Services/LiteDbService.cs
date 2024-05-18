using System.Text;
using System.Text.Json;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Models;
using Humanizer;
using LiteDB;

namespace DesktopAssistant.Services;

/// <summary>
/// LiteDB
/// アプリ固有の操作はなるべく実装しないように
/// </summary>
public class LiteDbService(ILocalSettingsService localSettingsService) : ILiteDbService
{
    private readonly string _filename = "data.db";
    private string FilePath => Path.Combine(localSettingsService.ApplicationDataFolder, _filename);
    private LiteDatabase GetContext => new(FilePath);

    public void CreateOrInitializeDatabase()
    {
        var localPath = localSettingsService.ApplicationDataFolder;
        if (IsExistDatabase())
        {
            //データベースファイルが存在している場合は削除
            File.Delete(Path.Combine(localPath, _filename));
        }
    }

    public bool IsExistDatabase() => File.Exists(FilePath);

    public IEnumerable<T> GetTable<T>()
    {
        using var context = GetContext;
        var table = context.GetCollection<T>(typeof(T).Name.Pluralize());
        return table.FindAll().ToList();
    }

    public void Upsert<T>(T data)
    {
        using var context = GetContext;
        var table = context.GetCollection<T>(typeof(T).Name.Pluralize());
        table.Upsert(data);
    }

    public void Delete<T>(T target) where T : IIdentifiable
    {
        using var context = GetContext;
        var table = context.GetCollection<T>(typeof(T).Name.Pluralize());
        table.Delete(target.Id);
    }

    // IIdentifiableが無ければ-1を返す
    public long GetLastId<T>()
    {
        // TがIIdentifiableを実装しているか
        return typeof(T).GetInterfaces().Contains(typeof(IIdentifiable)) ? GetTable<T>().Max(x => (x as IIdentifiable)!.Id) : -1;
    }
}

