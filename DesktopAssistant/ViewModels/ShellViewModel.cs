using CommunityToolkit.Mvvm.ComponentModel;

using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Views;

using Microsoft.UI.Xaml.Navigation;

namespace DesktopAssistant.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        // 現在選択されているものを設定する
        if (e.SourcePageType == typeof(SettingsPage))
        {
            // 設定画面の場合
            // ナビゲーションビューの設定アイテムを選択
            Selected = NavigationViewService.SettingsItem;
            return;
        }
        // 設定画面ではない場合
        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}
