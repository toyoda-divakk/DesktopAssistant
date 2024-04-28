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
public class LiteDbService : ILiteDbService
{
    private readonly ILocalSettingsService _localSettingsService;
    private readonly string _filename = "data.db";
    private string FilePath => Path.Combine(_localSettingsService.ApplicationDataFolder, _filename);
    private LiteDatabase GetContext => new(FilePath);

    public LiteDbService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;

        // 無視するフィールドなどを設定する
        var mapper = BsonMapper.Global;
        mapper.Entity<TaskCategory>()
            .Ignore(x => x.EditCommand)
            .Ignore(x => x.DeleteCommand);
    }

    public void CreateOrInitializeDatabase()
    {
        var localPath = _localSettingsService.ApplicationDataFolder;
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

    public void Insert<T>(T data)
    {
        using var context = GetContext;
        var table = context.GetCollection<T>(typeof(T).Name.Pluralize());
        table.Insert(data);
    }

    public void Delete<T>(T target) where T : IIdentifiable
    {
        using var context = GetContext;
        var table = context.GetCollection<T>(typeof(T).Name.Pluralize());
        table.Delete(target.Id);
    }
}
