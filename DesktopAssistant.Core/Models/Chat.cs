using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAssistant.Core.Models;

// TODO: ユーザと誰の会話かを区別するためのフィールドを追加する
// TODO: 誰のどの会話かを区別するためのTopicクラスを追加して、そのクラスをChatクラスに追加する
/// <summary>
/// 会話
/// </summary>
public record Chat
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id
    {
        get; init;
    }

    /// <summary>
    /// キャラクターID
    /// ユーザーの発言の場合はnull
    /// </summary>
    public long? CharacterId
    {
        get; init;
    }

    /// <summary>
    /// 内容
    /// </summary>
    public string Text
    {
        get; init;
    }

    /// <summary>
    /// 発言日時
    /// </summary>
    public DateTime CreatedAt
    {
        get; init;
    }
}
