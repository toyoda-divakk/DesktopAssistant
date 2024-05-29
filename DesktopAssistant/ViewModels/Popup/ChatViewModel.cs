using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Enums;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Core.Services;
using DesktopAssistant.Services;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.Chat;

namespace DesktopAssistant.ViewModels.Popup;

// 別ウィンドウで設定変更された場合、ここに伝える？どうやって？
// →面倒なので設定反映ボタンを付けるとか、閉じちゃうとか。
// →開きっぱなしでAI設定変更されたら流石にまずいので、チャット画面開いたまま変更画面の表示は出来ないようにする。

public partial class ChatViewModel(IChatSettingService chatSettingService, ILiteDbService liteDbService, IApiSettingService apiSettingService, ISemanticService semanticService) : ObservableRecipient, IChatSetting
{
    private Window _window = null!;         // サイアログのモーダル表示に必要なんだけど、要るかな？
    private Assistant _assistant = null!;
    private ChatHistory _chatHistory = null!;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(Window window)
    {
        // LiteDBから現在選択中のアシスタントの情報を取得する
        var assistant = liteDbService.GetAssistants().First(x => x.IsSelected);
        _assistant = assistant;
        _window = window;
        _chatHistory = semanticService.InitializeChat(apiSettingService.GetSettings(), _assistant.Prompt);
    }

    [ObservableProperty]
    private string userMessage = string.Empty;

    /// <summary>
    /// 送信可能
    /// </summary>
    [ObservableProperty]
    private bool isEnableMessage = true;

    public ObservableCollection<ChatMessage> ChatMessages { get; } = [];

    #region チャット表示設定を反映するためのプロパティ
    public ChatPosition UserPosition => chatSettingService.UserPosition;

    public string UserDisplayName => chatSettingService.UserDisplayName;

    public string UserBackgroundColor => chatSettingService.UserBackgroundColor;

    public string UserTextColor => chatSettingService.UserTextColor;       // TODO:

    public ChatPosition AIPosition => chatSettingService.AIPosition;

    // TODO:余裕があればやってみること…むり？
    public EnterKeyBond KeyBindNewLine => chatSettingService.KeyBindNewLine;

    // TODO:こっちはやってほしい。
    public EnterKeyBond KeyBindSend => chatSettingService.KeyBindSend;

    #endregion

    private string _lastInput = ""; // 最後の入力は取っておく。

    /// <summary>
    /// メッセージ送信して、チャットを進める
    /// </summary>
    [RelayCommand]
    public async void SendMessage()
    {
        // ユーザメッセージをリストに追加  
        ChatMessages.Add(new ChatMessage { UserName = UserDisplayName, Message = UserMessage, HorizontalAlignment = UserPosition.ToString(), BackgroundColor = UserBackgroundColor, TextColor = UserTextColor });
        IsEnableMessage = false;
        _lastInput = UserMessage;

        // AIのレスポンスを生成する処理をここに追加  
        var answer = await semanticService.GenerateChatAsync(_chatHistory, UserMessage);
        if (string.IsNullOrWhiteSpace(answer))
        {
            // 失敗した場合、最後のユーザ入力を削除する
            semanticService.RemoveLastMessage(_chatHistory);
            ChatMessages.RemoveAt(ChatMessages.Count - 1);
            return;
        }

        // 画面に反映
        ChatMessages.Add(new ChatMessage { UserName = _assistant.Name, Message = answer, HorizontalAlignment = AIPosition.ToString(), BackgroundColor = _assistant.BackColor, TextColor = _assistant.TextColor });

        // 成功したらメッセージボックスをクリア
        UserMessage = string.Empty;
        IsEnableMessage = true;

    }

    // リトライ：ChatMessagesと_chatHistoryの最後を削除する
    [RelayCommand]
    public void Retry()
    {
        // ChatMessagesが1件以下なら何もせずに終了
        if (ChatMessages.Count <= 1)
        {
            return;
        }

        semanticService.RemoveLastMessage(_chatHistory);
        ChatMessages.RemoveAt(ChatMessages.Count - 1);
        if (ChatMessages.Last().UserName == UserDisplayName)
        {
            ChatMessages.RemoveAt(ChatMessages.Count - 1);
            UserMessage = _lastInput;
        }
    }

    // TODO:最初からチャットをやり直すコマンドを追加する
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
    public string? TextColor
    {
        get; set;
    }
}
