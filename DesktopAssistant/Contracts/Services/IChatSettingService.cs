using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Enums;

namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// チャット画面設定を提供
/// </summary>
public interface IChatSettingService
{
    /// <summary>
    /// ユーザの発言の表示場所
    /// </summary>
    ChatPosition UserPosition
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
    /// AIの発言の表示場所
    /// </summary>
    ChatPosition AIPosition
    {
        get;
    }

    // TODO:実装は後にしよう
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

    /// <summary>
    /// 初期化処理
    /// ActivationServiceに登録すること
    /// </summary>
    /// <returns></returns>
    Task InitializeAsync();

    /// <summary>
    /// 変更内容を保存し、アプリに反映する
    /// </summary>
    /// <param name="generativeAI"></param>
    /// <returns></returns>
    Task SetAndSaveAsync(IChatSetting setting);

    /// <summary>
    /// 設定内容を直ちにアプリに反映する
    /// （特に何もしていない）
    /// </summary>
    /// <returns></returns>
    Task SetRequestedSettingAsync();
}
