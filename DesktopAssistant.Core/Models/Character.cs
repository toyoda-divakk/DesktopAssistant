using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiteDB;

namespace DesktopAssistant.Core.Models;

// Character -> Topic -> Message
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
        get; set;
    }

    /// <summary>
    /// 名前
    /// </summary>
    public string Name
    {
        get; set;
    }

    /// <summary>
    /// 見出し・説明
    /// </summary>
    public string Description
    {
        get; set;
    }

    /// <summary>
    /// プロンプト
    /// </summary>
    public string Prompt
    {
        get; set;
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
    /// 画像パス
    /// </summary>
    public string FaceImagePath
    {
        get; set;
    }

    //[BsonIgnore]やっぱ保存する
    /// <summary>
    /// 現在使用中か
    /// </summary>
    public bool IsSelected
    {
        get; set;
    }

    /// <summary>
    /// 会話のリスト
    /// </summary>
    [BsonIgnore]
    public ICollection<Topic> Topics
    {
        get; set;
    } = [];

    [BsonIgnore]
    public ICommand EditCommand
    {
        get; set;
    }

    [BsonIgnore]
    public ICommand CopyCommand
    {
        get; set;
    }

    [BsonIgnore]
    public ICommand DeleteCommand
    {
        get; set;
    }

    /// <summary>
    /// キャラクター切り替えコマンド
    /// </summary>
    [BsonIgnore]
    public ICommand SwitchCommand
    {
        get; set;
    }
}
