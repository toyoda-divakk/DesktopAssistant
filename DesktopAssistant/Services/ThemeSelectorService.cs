using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Helpers;

using Microsoft.UI.Xaml;

namespace DesktopAssistant.Services;

// これって値1個読み書きするだけだよね…。
/// <summary>
/// テーマの選択を管理するサービスを表します。
/// </summary>
public class ThemeSelectorService(ILocalSettingsService localSettingsService) : IThemeSelectorService
{
    private const string SettingsKey = "AppBackgroundRequestedTheme";

    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    public async Task InitializeAsync()
    {
        Theme = LoadThemeFromSettings();
        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        SaveThemeInSettings(Theme);  // 切り替えたらすぐにファイル保存
    }

    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(Theme);
        }

        await Task.CompletedTask;
    }

    private ElementTheme LoadThemeFromSettings()
    {
        var themeName = localSettingsService.ReadSetting<string>(SettingsKey);

        if (Enum.TryParse(themeName, out ElementTheme cacheTheme))
        {
            return cacheTheme;
        }

        return ElementTheme.Default;
    }

    private void SaveThemeInSettings(ElementTheme theme)
    {
        localSettingsService.SaveSetting(SettingsKey, theme.ToString());
    }
}
