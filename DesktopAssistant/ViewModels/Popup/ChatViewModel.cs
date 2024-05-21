using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Enums;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Services;
using Windows.ApplicationModel.Chat;

namespace DesktopAssistant.ViewModels.Popup;

// 別ウィンドウで設定変更された場合、ここに伝える？どうやって？
// →面倒なので設定反映ボタンを付けるとか、閉じちゃうとか。
// →開きっぱなしでAI設定変更されたら流石にまずいので、チャット画面開いたまま変更画面の表示は出来ないようにする。

public partial class ChatViewModel(IChatSettingService chatSettingService) : ObservableRecipient, IChatSetting // TODO:ICharacterSetting
{
    /// <summary>
    /// チャット表示設定
    /// </summary>
    private readonly IChatSettingService _chatSettingService = chatSettingService;

    // TODO:アシスタント設定を追加する。選択アシスタントは1人。まずはアシスタント編集画面を作る。デフォルトは開発補助のプロンプトを付けて効率UPしよう

    [ObservableProperty]
    private string userMessage = string.Empty;

    public ObservableCollection<ChatMessage> ChatMessages { get; } = [];

    #region チャット表示設定を反映するためのプロパティ
    public ChatPosition UserPosition => _chatSettingService.UserPosition;

    public string UserDisplayName => _chatSettingService.UserDisplayName;

    public string UserBackgroundColor => _chatSettingService.UserBackgroundColor;

    public string UserTextColor => _chatSettingService.UserTextColor;       // TODO:

    public ChatPosition AIPosition => _chatSettingService.AIPosition;

    // TODO:余裕があればやってみること…むり？
    public EnterKeyBond KeyBindNewLine => _chatSettingService.KeyBindNewLine;

    // TODO:こっちはやってほしい。
    public EnterKeyBond KeyBindSend => _chatSettingService.KeyBindSend;

    #endregion

    [RelayCommand]
    public void SendMessage()
    {
        // ユーザメッセージをリストに追加  
        ChatMessages.Add(new ChatMessage { UserName = UserDisplayName, Message = UserMessage, HorizontalAlignment = UserPosition.ToString(), BackgroundColor = UserBackgroundColor });

        // AIのレスポンスを生成する処理をここに追加  
        // 簡単のため、ここではユーザのメッセージをエコーバックするようにします  
        ChatMessages.Add(new ChatMessage { UserName = "AI", Message = UserMessage, HorizontalAlignment = AIPosition.ToString(), BackgroundColor = "#FFA07A" });    // "#FFA07A"

        // メッセージボックスをクリア  
        UserMessage = string.Empty;
    }

    [RelayCommand]
    public void Retry()
    {
        // メッセージリストをクリア  
        ChatMessages.Clear();
    }
}

public class ChatMessage
{
    public string? UserName
    {
        get; set;
    }
    public string? Message
    {
        get; set;
    }
    public string? HorizontalAlignment
    {
        get; set;
    }
    public string? BackgroundColor
    {
        get; set;
    }
}
