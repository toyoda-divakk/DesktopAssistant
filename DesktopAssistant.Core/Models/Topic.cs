﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace DesktopAssistant.Core.Models;

/// <summary>
/// 会話のまとまり
/// </summary>
public record Topic : IIdentifiable
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id
    {
        get; init;
    }

    /// <summary>
    /// キャラクター
    /// </summary>
    public long CharacterId
    {
        get; init;
    }

    /// <summary>
    /// 話題
    /// </summary>
    public string Subject
    {
        get; init;
    }

    /// <summary>
    /// 内容
    /// </summary>
    [BsonIgnore]
    public List<Message> Messages
    {
        get; init;
    } = [];

    /// <summary>
    /// 作成日時
    /// </summary>
    public DateTime CreatedAt
    {
        get; init;
    }

    /// <summary>
    /// 最後の会話日時
    /// </summary>
    public DateTime UpdatedAt
    {
        get; init;
    }
}
