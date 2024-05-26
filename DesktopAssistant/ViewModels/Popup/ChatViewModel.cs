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
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.Chat;

namespace DesktopAssistant.ViewModels.Popup;

// 別ウィンドウで設定変更された場合、ここに伝える？どうやって？
// →面倒なので設定反映ボタンを付けるとか、閉じちゃうとか。
// →開きっぱなしでAI設定変更されたら流石にまずいので、チャット画面開いたまま変更画面の表示は出来ないようにする。

public partial class ChatViewModel(IChatSettingService chatSettingService, ILiteDbService liteDbService) : ObservableRecipient, IChatSetting
{
    private readonly IChatSettingService _chatSettingService = chatSettingService;
    private readonly ILiteDbService _liteDbService = liteDbService;

    private Window _window;         // サイアログのモーダル表示に必要なんだけど、要るかな？
    private Assistant _assistant;

    /// <summary>
    /// 初期化
    /// カテゴリのリストを読み込む
    /// カテゴリ内のタスクを読み込む
    /// </summary>
    public void Initialize(Window window)
    {
        var taskCategories = _liteDbService.GetTable<Assistant>().FirstOrDefault(x => x.IsSelected);
        //foreach (var category in taskCategories)
        //{
        //    SetupCategory(category);
        //    category.TodoTasks = _liteDbService.GetTable<TodoTask>().Where(x => x.CategoryId == category.Id).ToList();
        //    Categories.Add(category);
        //}
        //// LiteDBから現在選択中のアシスタントの情報を取得する
        //var assistants = _liteDbService.GetAssistants();

        //_assistant = assistant;
        _window = window;
    }

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
