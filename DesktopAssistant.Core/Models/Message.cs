﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAssistant.Core.Models;

/// <summary>
/// 会話のメッセージ
/// </summary>
public record Message : IIdentifiable
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id
    {
        get; init;
    }

    /// <summary>
    /// Topic
    /// </summary>
    public long TopicId
    {
        get; init;
    }

    /// <summary>
    /// アシスタントの発言ならTrue
    /// ユーザーの発言ならFalse
    /// </summary>
    public bool IsAssistantMessage
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
