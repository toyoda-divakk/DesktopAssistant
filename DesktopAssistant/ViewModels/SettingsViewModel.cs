using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Enums;
using DesktopAssistant.Core.Services;
using DesktopAssistant.Helpers;
using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace DesktopAssistant.ViewModels;

public partial class SettingsViewModel : ObservableRecipient, IApiSetting, IChatSetting
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IApiSettingService _apiSettingService;
    private readonly IChatSettingService _chatSettingService;
    private readonly ISemanticService _semanticService;

    #region テーマ
    /// <summary>
    /// テーマ
    /// </summary>
    [ObservableProperty]
    private ElementTheme _elementTheme;

    ///// <summary>
    ///// バージョン情報
    ///// </summary>
    //[ObservableProperty]
    //private string _versionDescription;

    /// <summary>
    /// テーマ切り替えコマンド
    /// ラジオボタンではバインドできるが、コンボボックスではできない
    /// </summary>
    [RelayCommand]
    private async void SwitchTheme(ElementTheme param)
    {
        ElementTheme = param;
        await _themeSelectorService.SetThemeAsync(param);
        ShowAndHideMessageAsync();
    }

    #endregion

    #region 生成AI
    /// <summary>
    /// 生成AIサービス切り替えコマンド
    /// </summary>
    [RelayCommand]
    private void SwitchGenerativeAI(GenerativeAI param)
    {
        if (GenerativeAI != param)
        {
            GenerativeAI = param;
        }
    }

    /// <summary>
    /// AI生成のサービス名
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOpenAI), nameof(IsAzureOpenAI))]
    private GenerativeAI _generativeAI;

    /// <summary>
    /// 生成AI設定保存コマンド
    /// </summary>
    [RelayCommand]
    private async void SaveGenerativeAI()
    {
        await _apiSettingService.SetAndSaveAsync(this);
        ShowAndHideApiMessageAsync();
    }

    /// <summary>
    /// 生成AI接続テストコマンド
    /// </summary>
    [RelayCommand]
    private async void TestGenerativeAI()
    {
        EnableTestButton = false;
        GenerateTestResult = await _semanticService.TestGenerativeAIAsync(this, "Hello".GetLocalized());
        EnableTestButton = true;
    }

    // 表示・非表示に使う
    public bool IsOpenAI => GenerativeAI == GenerativeAI.OpenAI;
    public bool IsAzureOpenAI => GenerativeAI == GenerativeAI.AzureOpenAI;

    /// <summary>
    /// OpenAIのAPIキー
    /// </summary>
    [ObservableProperty]
    private string _openAIKey = string.Empty;
    /// <summary>
    /// OpenAIのモデル名
    /// </summary>
    [ObservableProperty]
    private string _openAIModel = string.Empty;

    /// <summary>
    /// AzureOpenAIのAPIキー
    /// </summary>
    [ObservableProperty]
    private string _azureOpenAIKey = string.Empty;
    /// <summary>
    /// AzureOpenAIのデプロイメント名
    /// </summary>
    [ObservableProperty]
    private string _azureOpenAIModel = string.Empty;
    /// <summary>
    /// AzureOpenAIのエンドポイント
    /// </summary>
    [ObservableProperty]
    private string _azureOpenAIEndpoint = string.Empty;

    /// <summary>
    /// 接続テストの結果
    /// </summary>
    [ObservableProperty]
    private string _generateTestResult = string.Empty;

    /// <summary>
    /// テストボタンの有効化
    /// </summary>
    [ObservableProperty]
    private bool _enableTestButton = true;
    #endregion

    #region チャット表示設定
    [ObservableProperty]
    private ChatPosition _userPosition;

    [ObservableProperty]
    private string _userBackgroundColor = string.Empty;

    [ObservableProperty]
    private string _userTextColor = string.Empty;

    [ObservableProperty]
    private ChatPosition _aIPosition;

    [ObservableProperty]
    private EnterKeyBond _keyBindNewLine;

    [ObservableProperty]
    private EnterKeyBond _keyBindSend;

    /// <summary>
    /// 切り替えコマンド
    /// </summary>
    [RelayCommand]
    private void SwitchAIPosition(ChatPosition param)
    {
        if (AIPosition != param)
        {
            AIPosition = param;
        }
    }
    [RelayCommand]
    private void SwitchUserPosition(ChatPosition param)
    {
        if (UserPosition != param)
        {
            UserPosition = param;
        }
    }
    [RelayCommand]
    private void SwitchKeyBindNewLine(EnterKeyBond param)
    {
        if (KeyBindNewLine != param)
        {
            KeyBindNewLine = param;
        }
    }
    [RelayCommand]
    private void SwitchKeyBindSend(EnterKeyBond param)
    {
        if (KeyBindSend != param) // TODO:KeyBindNewLineと同じ値にしない事
        {
            KeyBindSend = param;
        }
    }

    /// <summary>
    /// チャット表示設定保存コマンド
    /// </summary>
    [RelayCommand]
    private async void SaveChat()
    {
        await _chatSettingService.SetAndSaveAsync(this);
        ShowAndHideChatMessageAsync();
    }
    #endregion

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IApiSettingService apiSettingService, IChatSettingService chatSettingService, ISemanticService semanticService)
    {
        _isVisibleMessage = false;
        _isVisibleApiMessage = false;
        _isVisibleChatMessage = false;

        // サービスの初期化
        _themeSelectorService = themeSelectorService;
        _apiSettingService = apiSettingService;
        _chatSettingService = chatSettingService;
        _semanticService = semanticService;

        // 設定の再読み込み
        _apiSettingService.InitializeAsync();
        FieldCopier.CopyProperties<IApiSetting>(_apiSettingService, this);
        _chatSettingService.InitializeAsync();
        FieldCopier.CopyProperties<IChatSetting>(_chatSettingService, this);

        // 表示設定
        _elementTheme = _themeSelectorService.Theme;
        //_versionDescription = GetVersionDescription();

    }

    #region 保存メッセージ表示
    // 本当はRefにしたいけどasyncだから無理
    /// <summary>
    /// 保存メッセージの表示
    /// </summary>
    [ObservableProperty]
    private bool _isVisibleMessage;
    [ObservableProperty]
    private bool _isVisibleApiMessage;
    [ObservableProperty]
    private bool _isVisibleChatMessage;

    /// <summary>
    /// 3秒後にメッセージを非表示にする
    /// 一番上のメッセージ表示用
    /// </summary>
    private async void ShowAndHideMessageAsync()
    {
        IsVisibleMessage = true;
        await Task.Delay(3000);
        IsVisibleMessage = false;
    }

    /// <summary>
    /// 3秒後にメッセージを非表示にする
    /// API保存用
    /// </summary>
    private async void ShowAndHideApiMessageAsync()
    {
        IsVisibleApiMessage = true;
        await Task.Delay(3000);
        IsVisibleApiMessage = false;
    }

    /// <summary>
    /// 3秒後にメッセージを非表示にする
    /// チャット設定保存用
    /// </summary>
    private async void ShowAndHideChatMessageAsync()
    {
        IsVisibleChatMessage = true;
        await Task.Delay(3000);
        IsVisibleChatMessage = false;
    }
    #endregion

    ///// <summary>
    ///// バージョン情報を取得
    ///// </summary>
    ///// <returns>バージョン情報</returns>
    //private static string GetVersionDescription()
    //{
    //    Version version;

    //    if (RuntimeHelper.IsMSIX)
    //    {
    //        // パッケージからバージョン情報を取得
    //        var packageVersion = Package.Current.Id.Version;
    //        version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
    //    }
    //    else
    //    {
    //        // アセンブリからバージョン情報を取得
    //        version = Assembly.GetExecutingAssembly().GetName().Version!;
    //    }

    //    // アプリ名とバージョン情報を結合して返す
    //    return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    //}
}
