using DesktopAssistant.Activation;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Services;

// ActivationHandlerを使用することで、アプリケーションの起動時に特定のアクティベーション引数を処理するための柔軟性と拡張性を実現することができます。
/// <summary>
/// アプリケーションの起動時に実行されるさまざまなタスクを処理する
/// アプリケーションの初期化
/// アクティベーションハンドラの処理
/// テーマの設定など
/// </summary>
public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IThemeSelectorService _themeSelectorService;
    private UIElement? _shell = null;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, IThemeSelectorService themeSelectorService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
    }

    /// <summary>
    /// アプリケーションの起動時に呼び出される
    /// </summary>
    /// <param name="activationArgs">アクティベーション引数</param>
    /// <returns></returns>
    public async Task ActivateAsync(object activationArgs)
    {
        // アクティベーション前のタスク
        await InitializeAsync();

        // MainWindowのコンテンツの設定
        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<ShellPage>();
            App.MainWindow.Content = _shell ?? new Frame();
        }

        await HandleActivationAsync(activationArgs);

        // MainWindowのアクティベーション
        App.MainWindow.Activate();

        // StartupAsyncメソッドの呼び出し
        await StartupAsync();
    }

    /// <summary>
    /// アクティベーション引数を処理
    /// 与えられたアクティベーション引数に対して、登録されたActivationHandlerの中から適切なハンドラを見つけて実行します。
    /// </summary>
    /// <param name="activationArgs">アクティベーション引数</param>
    /// <returns></returns>
    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    /// <summary>
    /// 最初に実行
    /// テーマの初期化など
    /// </summary>
    /// <returns></returns>
    private async Task InitializeAsync()
    {
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    /// <summary>
    /// 最後に実行
    /// </summary>
    /// <returns></returns>
    private async Task StartupAsync()
    {
        await _themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
    }
}
