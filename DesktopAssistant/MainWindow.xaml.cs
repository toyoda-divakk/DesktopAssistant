using DesktopAssistant.Helpers;

using Windows.UI.ViewManagement;

namespace DesktopAssistant;

public sealed partial class MainWindow : WindowEx
{
    /// <summary>
    /// UIスレッドで非同期操作を実行する仕組み
    /// UIスレッドはシングルスレッドなのでこのような仕組みが必要
    /// </summary>
    private readonly Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;

    private readonly UISettings settings;

    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        settings = new UISettings();
        settings.ColorValuesChanged += Settings_ColorValuesChanged; // FrameworkElement.ActualThemeChangedイベントは使用できません。
    }

    // これは、ウィンドウズのシステムテーマが変更されたときに、キャプションボタンの色を正しく更新する処理です。アプリが開いている間
    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // この呼び出しはオフスレッドで行われるため、現在のアプリのスレッドにディスパッチする必要がある。
        dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }

    /// <summary>
    /// 他のウィンドウと一緒に閉じる
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void WindowEx_Closed(object sender, Microsoft.UI.Xaml.WindowEventArgs args)
    {
        var children = WindowHelper.ActiveWindows.ToArray();    // ToArrayしないと、foreach中にコレクションが変更されるため例外が発生する
        foreach (var child in children)
        {
            child.Close();
        }
        App.MainWindow.Close();
    }
}
