using System.Diagnostics.CodeAnalysis;

using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Helpers;
using DesktopAssistant.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Services;

/// <summary>
/// NavigationViewの操作を補助する
/// </summary>
public class NavigationViewService : INavigationViewService
{
    private readonly INavigationService _navigationService; // 実際のページのナビゲーション

    private readonly IPageService _pageService; // ページの型の取得

    private NavigationView? _navigationView;

    /// <summary>
    /// ナビゲーションビューのメニューアイテムのリスト
    /// </summary>
    public IList<object>? MenuItems => _navigationView?.MenuItems;

    /// <summary>
    /// ナビゲーションビューの設定アイテム
    /// </summary>
    public object? SettingsItem => _navigationView?.SettingsItem;

    public NavigationViewService(INavigationService navigationService, IPageService pageService)
    {
        _navigationService = navigationService;
        _pageService = pageService;
    }

    [MemberNotNull(nameof(_navigationView))]    // _navigationViewがnullでない値を持つことをIDEに対して保証する
    public void Initialize(NavigationView navigationView)
    {
        _navigationView = navigationView;
        _navigationView.BackRequested += OnBackRequested;   // ナビゲーションビュー内の戻るボタンが押されたときに実行されるイベントハンドラ
        _navigationView.ItemInvoked += OnItemInvoked;   // ナビゲーションビュー内のアイテムが選択されたときに実行されるイベントハンドラ
    }

    public void UnregisterEvents()
    {
        if (_navigationView != null)
        {
            _navigationView.BackRequested -= OnBackRequested;
            _navigationView.ItemInvoked -= OnItemInvoked;
        }
    }

    public NavigationViewItem? GetSelectedItem(Type pageType)
    {
        if (_navigationView != null)
        {
            return GetSelectedItem(_navigationView.MenuItems, pageType) ?? GetSelectedItem(_navigationView.FooterMenuItems, pageType);
        }

        return null;
    }

    /// <summary>
    /// 前に戻る
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => _navigationService.GoBack();

    /// <summary>
    /// ナビゲーションビュー内のアイテムが選択されたときに実行
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            // 設定アイテムが選択されたとき
            _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
        }
        else
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem?.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
            {
                _navigationService.NavigateTo(pageKey);
            }
        }
    }

    private NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
    {
        foreach (var item in menuItems.OfType<NavigationViewItem>())
        {
            if (IsMenuItemForPageType(item, pageType))
            {
                return item;
            }

            var selectedChild = GetSelectedItem(item.MenuItems, pageType);
            if (selectedChild != null)
            {
                return selectedChild;
            }
        }

        return null;
    }

    /// <summary>
    /// NavigationViewItemオブジェクトが指定されたページの型に対応しているかどうかを判定します。
    /// </summary>
    /// <param name="menuItem"></param>
    /// <param name="sourcePageType"></param>
    /// <returns></returns>
    private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
    {
        if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
        {
            // ページのキーが一致するかどうかを判定
            return _pageService.GetPageType(pageKey) == sourcePageType;
        }

        return false;
    }
}
