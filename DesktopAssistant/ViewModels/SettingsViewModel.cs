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

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IApiSettingService _apiSettingService;

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
    /// AI生成
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOpenAI))]
    [NotifyPropertyChangedFor(nameof(IsAzureOpenAI))]
    private GenerativeAI _generativeAI;

    /// <summary>
    /// AI生成切り替えコマンド
    /// </summary>
    public ICommand SwitchGenerativeAICommand
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
    private string _openAIKey;
    /// <summary>
    /// OpenAIのモデル名
    /// </summary>
    [ObservableProperty]
    private string _openAIModel;

    /// <summary>
    /// AzureOpenAIのAPIキー
    /// </summary>
    [ObservableProperty]
    private string _azureOpenAIKey;
    /// <summary>
    /// AzureOpenAIのデプロイメント名
    /// </summary>
    [ObservableProperty]
    private string _azureOpenAIModel;
    /// <summary>
    /// AzureOpenAIのエンドポイント
    /// </summary>
    [ObservableProperty]
    private string _azureOpenAIEndpoint;


    public SettingsViewModel(IThemeSelectorService themeSelectorService, IApiSettingService apiSettingService)
    {
        // サービスの初期化
        _themeSelectorService = themeSelectorService;
        _apiSettingService = apiSettingService;

        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });

        _generativeAI = _apiSettingService.GenerativeAI;
        SwitchGenerativeAICommand = new RelayCommand<GenerativeAI>(
            async (param) =>
            {
                if (GenerativeAI != param)
                {
                    GenerativeAI = param;
                    await _apiSettingService.SetGenerativeAIAsync(param);
                }
            });
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
