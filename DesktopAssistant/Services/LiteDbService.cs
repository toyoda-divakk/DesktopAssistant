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
    private readonly ILocalSettingsService _localSettingsService = localSettingsService;
    private readonly string _filename = "data.db";
    private string FilePath => Path.Combine(_localSettingsService.ApplicationDataFolder, _filename);
    private LiteDatabase GetContext => new(FilePath);

    public void CreateOrInitializeDatabase()
    {
        var localPath = _localSettingsService.ApplicationDataFolder;
        if (IsExistDatabase())
        {
            //データベースファイルが存在している場合は削除
            File.Delete(Path.Combine(localPath, _filename));
        }
        using var context = new LiteDatabase(Path.Combine(localPath, _filename));
        context.GetCollection<TodoTask>("TodoTasks");
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

    //public List<TodoTask> Test()
    //{
    //    // コンストラクタ引数はファイル名
    //    using var context = GetContext;

    //    // エンティティ
    //    var todoTask = new TodoTask()
    //    {
    //        Title = "たいとるうううう",
    //        Id = 0,
    //    };

    //    // DBへ接続
    //    var todoTasks = context.GetCollection<TodoTask>("TodoTasks");

    //    // ユニークインデックスの設定
    //    todoTasks.EnsureIndex(x => x.Id, true);

    //    // データが無かったら作成
    //    todoTasks.Upsert(todoTask);

    //    todoTask.Title = "更新したよおおおお";

    //    // 更新
    //    todoTasks.Update(todoTask);

    //    // 検索
    //    // Titleが「更」で始まるもの
    //    var results = todoTasks.Find(x => x.Title.StartsWith("更")).ToList();
    //    return results;
    //}
}
