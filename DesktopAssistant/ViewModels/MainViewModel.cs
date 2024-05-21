using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Helpers;
using DesktopAssistant.Views.Popup;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;

namespace DesktopAssistant.ViewModels;

public partial class MainViewModel(IThemeSelectorService themeSelector, ILocalSettingsService localSettings) : ObservableRecipient
{
    private IThemeSelectorService ThemeSelector { get; } = themeSelector;
    private ILocalSettingsService LocalSettings { get; } = localSettings;

    /// <summary>
    /// ToDoListPageをモードレス表示する
    /// </summary>
    [RelayCommand]
    private void ShowToDoList()
    {
        var newWindow = WindowHelper.CreateWindow();
        newWindow.SetWindowSize(860, 600);
        newWindow.Title = "ToDoList".GetLocalized();
        var rootPage = new ToDoListPage
        {
            RequestedTheme = ThemeSelector.Theme
        };
        newWindow.Content = rootPage;
        rootPage.ViewModel.Initialize(newWindow);
        newWindow.Activate();
    }

    /// <summary>
    /// ChatPageをモードレス表示する
    /// </summary>
    [RelayCommand]
    private void Chat()
    {
        var newWindow = WindowHelper.CreateWindow();
        newWindow.SetWindowSize(860, 600);
        newWindow.Title = "Chat".GetLocalized();
        var rootPage = new ChatPage
        {
            RequestedTheme = ThemeSelector.Theme
        };
        newWindow.Content = rootPage;
        //rootPage.ViewModel.Initialize(newWindow);
        newWindow.Activate();
    }

    /// <summary>
    /// AssistantPageをモードレス表示する
    /// </summary>
    [RelayCommand]
    private void ShowAssistant()
    {
        var newWindow = WindowHelper.CreateWindow();
        newWindow.SetWindowSize(860, 600);      // TODO:画像ファイルからサイズを決定すること
        newWindow.Title = "AssistantImage".GetLocalized();
        var rootPage = new AssistantImagePage
        {
            RequestedTheme = ThemeSelector.Theme
        };
        newWindow.Content = rootPage;
        newWindow.Activate();
    }

    /// <summary>
    /// データフォルダのパスをクリップボードにコピーする
    /// </summary>
    [RelayCommand]
    private void ShowDataFolder()
    {
        // アクセス権がないので開くことができない
        var path = LocalSettings.ApplicationDataFolder;
        var package = new DataPackage();
        package.SetText(path);
        Clipboard.SetContent(package);

    }

    /// <summary>
    /// 全てのWindowを閉じる
    /// </summary>
    [RelayCommand]
    private static void Close()
    {
        var children = WindowHelper.ActiveWindows.ToArray();    // ToArrayしないと、foreach中にコレクションが変更されるため例外が発生する
        foreach (var child in children)
        {
            child.Close();
        }
        App.MainWindow.Close();
    }
}

