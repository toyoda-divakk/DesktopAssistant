﻿using DesktopAssistant.Core.Enums;

namespace DesktopAssistant.Core.Contracts.Interfaces;

/// <summary>
/// チャット画面に関する設定項目のインターフェース
/// </summary>
public interface IChatSetting
{
    /// <summary>
    /// 自分が右か左か
    /// </summary>
    ChatPosition UserPosition
    {
        get;
    }
    /// <summary>
    /// ユーザーの表示名
    /// </summary>
    string UserDisplayName
    {
        get;
    }
    /// <summary>
    /// ユーザーの吹き出し色
    /// </summary>
    string UserBackgroundColor
    {
        get;
    }
    /// <summary>
    /// ユーザーの文字色
    /// ユーザーの吹き出し色から白か黒を決定する
    /// </summary>
    string UserTextColor
    {
        get;
    }

    /// <summary>
    /// AIが右か左か
    /// </summary>
    ChatPosition AIPosition
    {
        get;
    }

    /// <summary>
    /// 改行のキーバインド
    /// デフォルトはEnter
    /// </summary>
    EnterKeyBond KeyBindNewLine
    {
        get;
    }
    /// <summary>
    /// 送信のキーバインド
    /// デフォルトはCtrl + Enter
    /// </summary>
    EnterKeyBond KeyBindSend
    {
        get;
    }
}

