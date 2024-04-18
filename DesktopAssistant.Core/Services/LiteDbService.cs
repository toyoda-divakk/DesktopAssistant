//using System.Text;
//using System.Text.Json;
//using DesktopAssistant.Core.Contracts.Services;
//using DesktopAssistant.Core.Models;
//using Humanizer;
//using LiteDB;

//namespace DesktopAssistant.Core.Services;

///// <summary>
///// LiteDB
///// アプリ固有の操作はなるべく実装しないように
///// </summary>
//public class LiteDbService : ILiteDbService
//{
//    private readonly string _filename = "data.db";

//    public void CreateOrInitializeDatabase(string localPath)
//    {
//        if (IsExistDatabase(localPath))
//        {
//            //データベースファイルが存在している場合は削除
//            File.Delete(Path.Combine(localPath, _filename));
//        }
//        using var context = new LiteDatabase(Path.Combine(localPath, _filename));
//        context.GetCollection<TodoTask>("TodoTasks");
//    }

//    public bool IsExistDatabase(string localPath) => File.Exists(Path.Combine(localPath, _filename));

//    public List<T> GetTable<T>(string folder)
//    {
//        using var context = new LiteDatabase(Path.Combine(folder, _filename));
//        var test = nameof(T).Pluralize();
//        var table = context.GetCollection<T>(nameof(T).Pluralize());
//        return table.FindAll().ToList();
//    }

//    public void Insert<T>(T data)
//    {
//        using var context = new LiteDatabase(_filename);    // TODO:localPathは覚えてもらって、各メソッドの引数から消す。そのためにはCoreからメインにサービスを移動させて、設定値を注入する
//    }

//    public void AddSystemEvent(SystemEvents systemEvent, string content)
//    {
//        using var context = new LiteDatabase(_filename);
//        var systemEvents = context.GetCollection<SystemEvent>("SystemEvents");

//        var systemEventEntity = new SystemEvent()
//        {
//            Event = systemEvent,
//            Content = content,
//            IsDone = false,
//        };

//        systemEvents.Insert(systemEventEntity);
//    }

//    public List<TodoTask> Test(string folder)
//    {
//        // コンストラクタ引数はファイル名
//        using var context = new LiteDatabase(Path.Combine(folder, _filename));

//        // エンティティ
//        var todoTask = new TodoTask()
//        {
//            Title = "たいとるうううう",
//            Id = 0,
//        };

//        // DBへ接続
//        var todoTasks = context.GetCollection<TodoTask>("TodoTasks");

//        // ユニークインデックスの設定
//        todoTasks.EnsureIndex(x => x.Id, true);

//        // データが無かったら作成
//        todoTasks.Upsert(todoTask);

//        todoTask.Title = "更新したよおおおお";

//        // 更新
//        todoTasks.Update(todoTask);

//        // 検索
//        // Titleが「更」で始まるもの
//        var results = todoTasks.Find(x => x.Title.StartsWith("更")).ToList();
//        return results;
//    }
//}
