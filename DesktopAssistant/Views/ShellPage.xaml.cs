using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Helpers;
using DesktopAssistant.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

using Windows.System;

namespace DesktopAssistant.Views;

// TODO: ShellPage.xamlのNavigationViewItemのタイトルとアイコンを更新すること

/// <summary>
/// アプリケーションのシェルページ
/// タイトルバーのカスタマイズを行っている
/// ナビゲーションメニューと各画面の表示指定を行っている
/// キーボードショートカットで「戻る」ができるようにしている
/// </summary>
public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel
    {
        get;
    }

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        // xaml右側
        ViewModel.NavigationService.Frame = NavigationFrame;                // "NavigationFrame"という名前のFrameを追加しているので、そこに各ページを表示するように設定
        // xaml左側
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);  // "NavigationViewControl"という名前のNavigationViewを追加しているので、そこに各ページのナビゲーションを表示するように設定

        // TODO: Assets/WindowIcon.icoを更新してタイトルバーアイコンを設定します。
        // フルウィンドウテーマとMicaをサポートするには、カスタムタイトルバーが必要です。
        // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization
        // Micaとは: アプリや設定などの長時間のウィンドウの背景を塗り分け、テーマとデスクトップの壁紙を組み込んだ不透明で動的な素材

        // タイトルバーのカスタマイズ
        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);  // テーマでタイトルバーの色を更新する

        // キーボードアクセラレータの追加（キーボードショートカットを作る）
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        // ウィンドウがアクティブになったときに、タイトルバーの色を更新する？
        App.AppTitlebar = AppTitleBarText;
    }

    /// <summary>
    /// 表示モードがMinimalかそうでないかで、タイトルバーの左側のマージンを変更する
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    /// <summary>
    /// キーボードショートカットの設定
    /// 指定されたキーを入力したときに、NavigationService.GoBack()を呼び出す
    /// </summary>
    /// <param name="key"></param>
    /// <param name="modifiers"></param>
    /// <returns></returns>
    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    /// <summary>
    /// NavigationService.GoBack()を呼び出すイベントハンドラ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }
}
