using System.Text;
using System.Text.Json;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;
using LiteDB;

namespace DesktopAssistant.Core.Services;

/// <summary>
/// LiteDB
/// </summary>
public class LiteDbService : ILiteDbService
{
    public List<TodoTask> Test(string folder) {
        // コンストラクタ引数はファイル名
        using var context = new LiteDatabase(Path.Combine(folder, "data.db"));

        // エンティティ
        var todoTask = new TodoTask()
        {
            Title = "たいとるうううう",
            Id = 0,
        };

        // DBへ接続
        var todoTasks = context.GetCollection<TodoTask>("TodoTasks");

        // ユニークインデックスの設定
        todoTasks.EnsureIndex(x => x.Id, true);

        // データが無かったら作成
        todoTasks.Upsert(todoTask);

        todoTask.Title = "更新したよおおおお";

        // 更新
        todoTasks.Update(todoTask);

        // 検索
        // Titleが「更」で始まるもの
        var results = todoTasks.Find(x => x.Title.StartsWith("更")).ToList();
        return results;
    }
}
