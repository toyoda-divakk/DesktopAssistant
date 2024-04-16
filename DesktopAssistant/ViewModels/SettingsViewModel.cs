using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Enums;
using DesktopAssistant.Helpers;
using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace DesktopAssistant.ViewModels;

public partial class SettingsViewModel : ObservableRecipient, IApiSetting
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IApiSettingService _apiSettingService;

    /// <summary>
    /// 保存メッセージの表示
    /// </summary>
    [ObservableProperty]
    private bool _isVisibleMessage;

    /// <summary>
    /// テーマ
    /// </summary>
    [ObservableProperty]
    private ElementTheme _elementTheme;

    /// <summary>
    /// バージョン情報
    /// </summary>
    [ObservableProperty]
    private string _versionDescription;

    /// <summary>
    /// テーマ切り替えコマンド
    /// </summary>
    public ICommand SwitchThemeCommand
    {
        get;
    }

    /// <summary>
    /// AI生成のサービス名
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOpenAI), nameof(IsAzureOpenAI))]
    private GenerativeAI _generativeAI;

    /// <summary>
    /// AI生成切り替えコマンド
    /// </summary>
    public ICommand SaveGenerativeAICommand
    {
        get;
    }
    // https://devlog.mescius.jp/dotnet-community-toolkit-mvvm-command/
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


    public SettingsViewModel(IThemeSelectorService themeSelectorService, IApiSettingService apiSettingService)
    {
        // サービスの初期化
        _themeSelectorService = themeSelectorService;
        _apiSettingService = apiSettingService;

        // 設定の再読み込み
        _apiSettingService.InitializeAsync();

        // 表示設定
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        // テーマ切り替え処理
        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                    ShowAndHideMessageAsync();
                }
            }
        );

        // 生成AIの設定
        FieldCopier.CopyProperties<IApiSetting>(_apiSettingService, this);
        //_generativeAI = _apiSettingService.GenerativeAI;
        //_openAIKey = _apiSettingService.OpenAIKey;
        //_openAIModel = _apiSettingService.OpenAIModel;
        //_azureOpenAIKey = _apiSettingService.AzureOpenAIKey;
        //_azureOpenAIModel = _apiSettingService.AzureOpenAIModel;
        //_azureOpenAIEndpoint = _apiSettingService.AzureOpenAIEndpoint;
        _isVisibleMessage = false;

        // 保存ボタン処理
        SaveGenerativeAICommand = new RelayCommand(
            async () =>
            {
                await _apiSettingService.SetGenerativeAIAsync(this);
                ShowAndHideMessageAsync();
            }
        );
    }

    /// <summary>
    /// 3秒後にメッセージを非表示にする
    /// </summary>
    private async void ShowAndHideMessageAsync()
    {
        IsVisibleMessage = true;
        await Task.Delay(3000);
        IsVisibleMessage = false;
    }

    /// <summary>
    /// バージョン情報を取得
    /// </summary>
    /// <returns>バージョン情報</returns>
    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            // パッケージからバージョン情報を取得
            var packageVersion = Package.Current.Id.Version;
            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            // アセンブリからバージョン情報を取得
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        // アプリ名とバージョン情報を結合して返す
        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
