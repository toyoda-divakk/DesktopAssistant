using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// 画面遷移サービスのインターフェース
/// </summary>
public interface INavigationService
{
    event NavigatedEventHandler Navigated;

    bool CanGoBack
    {
        get;
    }

    Frame? Frame
    {
        get; set;
    }

    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);

    bool GoBack();

    // これなに？
    // https://learn.microsoft.com/ja-jp/windows/apps/design/motion/connected-animation
    void SetListDataItemForNextConnectedAnimation(object item); 
}
