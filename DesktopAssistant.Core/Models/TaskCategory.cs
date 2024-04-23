using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopAssistant.Core.Models;

/// <summary>
/// Todoタスクの分類
/// </summary>
public record TaskCategory
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id
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
    public List<TodoTask> TodoTasks
    {
        get; set;
    }

    // Ignoreなので、LiteDbServiceで個別に指定する
    public ICommand EditCommand
    {
        get; set;
    }
}
