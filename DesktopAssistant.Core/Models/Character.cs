using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAssistant.Core.Models;

// Character -> Topic -> Message
// NoSQLでは紐づけ方が分からないので、子オブジェクトは親のIdを持たせる
/// <summary>
/// キャラクター
/// </summary>
public record Character : IIdentifiable
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
    /// 名前
    /// </summary>
    public string Name
    {
        get; init;
    }

    /// <summary>
    /// 見出し・説明
    /// </summary>
    public string Description
    {
        get; init;
    }

    /// <summary>
    /// プロンプト
    /// </summary>
    public string Prompt
    {
        get; init;
    }

    /// <summary>
    /// 背景色
    /// </summary>
    public string BackColor
    {
        get; set;
    }

    /// <summary>
    /// 文字色
    /// </summary>
    public string TextColor
    {
        get; set;
    }

    /// <summary>
    /// 会話のリスト
    /// </summary>
    public ICollection<Topic> Topics
    {
        get; set;
    }
}
