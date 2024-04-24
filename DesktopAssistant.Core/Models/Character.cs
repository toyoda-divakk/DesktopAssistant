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
public record Character
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
    /// プロンプト
    /// </summary>
    public string Prompt
    {
        get; init;
    }

    /// <summary>
    /// 会話のリスト
    /// </summary>
    public ICollection<Topic> Topics
    {
        get; set;
    }
}
