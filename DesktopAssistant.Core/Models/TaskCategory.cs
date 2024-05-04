using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiteDB;

namespace DesktopAssistant.Core.Models;

/// <summary>
/// Todoタスクの分類
/// </summary>
public record TaskCategory : IIdentifiable
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id
    {
        get; init;
    }

    /// <summary>
    /// 表示順
    /// </summary>
    public int Order
    {
        get; init;
    }

    /// <summary>
    /// 分類名
    /// </summary>
    public string Name
    {
        get; set;
    }

    // ここにデータがある状態でDBに入れると、読み込み時に関連したデータも取得するみたい。どういう仕組み？
    /// <summary>
    /// 属するタスク
    /// </summary>
    [BsonIgnore]
    public List<TodoTask> TodoTasks
    {
        get; set;
    } = [];

    // ※あまりIgnoreみたいなのやりたくないんだけど、ListViewの右クリックメニューに直接処理を付けたい場合これが一番簡単。
    // Ignoreなので、LiteDbServiceで個別に指定する
    [BsonIgnore]
    public ICommand EditCommand
    {
        get; set;
    }
    [BsonIgnore]
    public ICommand DeleteCommand
    {
        get; set;
    }
}
