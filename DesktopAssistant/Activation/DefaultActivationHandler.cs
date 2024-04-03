using DesktopAssistant.Contracts.Services;
using DesktopAssistant.ViewModels;

using Microsoft.UI.Xaml;

namespace DesktopAssistant.Activation;

// 起動時の処理の指定方法
// アプリケーションが起動されたときに、まだ画面が表示されていない場合に限り、MainViewModelに遷移する処理を行う例

/// <summary>
/// 起動時の処理を行うActivationHandlerのデフォルト実装
/// </summary>
public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService; // 画面遷移を行うサービス

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    // 実行条件を書く
    /// <summary>
    /// 起動時の引数を受け取り、処理を行うかどうかを判断します。
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        return _navigationService.Frame?.Content == null;
    }

    // 実行内容を書く
    /// <summary>
    /// MainViewModelに遷移します。
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(MainViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
