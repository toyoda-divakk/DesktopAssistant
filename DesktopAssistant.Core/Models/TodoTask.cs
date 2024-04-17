using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAssistant.Core.Models;

/// <summary>
/// Todoタスク
/// </summary>
public record TodoTask
{
    /// <summary>
    /// タスクのID
    /// </summary>
    public long Id
    {
        get; init;
    }

    /// <summary>
    /// タスクの題名
    /// </summary>
    public string Title
    {
        get; set;
    }

    /// <summary>
    /// タスクの内容
    /// </summary>
    public string Content
    {
        get; set;
    }

    /// <summary>
    /// タスクの進捗メモ
    /// </summary>
    public string Progress
    {
        get; set;
    }

    /// <summary>
    /// タスクが完了しているかどうか
    /// </summary>
    public bool IsDone
    {
        get; set;
    }

    /// <summary>
    /// タスクの作成日時
    /// </summary>
    public DateTime CreatedAt
    {
        get; init;
    }

    /// <summary>
    /// タスクの更新日時
    /// </summary>
    public DateTime UpdatedAt
    {
        get; set;
    }

    /// <summary>
    /// タスクの期限
    /// </summary>
    public DateTime? Deadline
    {
        get; set;
    }
}
