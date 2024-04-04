using Microsoft.UI.Xaml;

namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// テーマの選択を提供
/// </summary>
public interface IThemeSelectorService
{
    ElementTheme Theme
    {
        get;
    }

    Task InitializeAsync();

    Task SetThemeAsync(ElementTheme theme);

    Task SetRequestedThemeAsync();
}
