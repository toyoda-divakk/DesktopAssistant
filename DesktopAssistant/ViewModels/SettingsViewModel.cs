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

public partial class SettingsViewModel : ObservableRecipient, IApiSetting
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IApiSettingService _apiSettingService;
    private readonly ISemanticService _semanticService;

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
    /// ラジオボタンではバインドできるが、コンボボックスではできない
    /// </summary>
    public ICommand SwitchThemeCommand
    {
        get;
    }

    /// <summary>
    /// 生成AIサービス切り替えコマンド
    /// </summary>
    public RelayCommand<GenerativeAI> SwitchGenerativeAICommand
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
    /// 生成AI設定保存コマンド
    /// </summary>
    public ICommand SaveGenerativeAICommand
    {
        get;
    }

    /// <summary>
    /// 生成AI接続テストコマンド
    /// </summary>
    public ICommand TestGenerativeAICommand
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

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IApiSettingService apiSettingService, ISemanticService semanticService)
    {
        // サービスの初期化
        _themeSelectorService = themeSelectorService;
        _apiSettingService = apiSettingService;
        _semanticService = semanticService;

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

        // AIサービス切り替え処理
        SwitchGenerativeAICommand = new RelayCommand<GenerativeAI>(
            (param) =>
            {
                if (GenerativeAI != param)
                {
                    GenerativeAI = param;
                }
            }
        );

        // 生成AIの設定
        FieldCopier.CopyProperties<IApiSetting>(_apiSettingService, this);
        _isVisibleMessage = false;

        // 保存ボタン処理
        SaveGenerativeAICommand = new RelayCommand(
            async () =>
            {
                await _apiSettingService.SetGenerativeAIAsync(this);
                ShowAndHideMessageAsync();
            }
        );

        // AI接続テスト処理
        TestGenerativeAICommand = new RelayCommand(
            async () =>
            {
                EnableTestButton = false;
                GenerateTestResult = await _semanticService.TestGenerativeAIAsync(this, "Hello".GetLocalized());
                EnableTestButton = true;
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
