using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Helpers;

// Frame クラスは、アプリケーションのメインウィンドウ内に配置され、ページのコンテンツを表示します。また、ページ間の遷移やナビゲーションの管理も行います。
public static class FrameExtensions
{
    /// <summary>
    /// Frame内に表示されているページのビューモデル（ViewModel）オブジェクトを取得します。
    /// </summary>
    /// <param name="frame"></param>
    /// <returns></returns>
    public static object? GetPageViewModel(this Frame frame) => frame?.Content?.GetType().GetProperty("ViewModel")?.GetValue(frame.Content, null);
}
