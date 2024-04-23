using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Helpers;
using DesktopAssistant.Views.Popup;

namespace DesktopAssistant.ViewModels;

public partial class MainViewModel(IThemeSelectorService themeSelector) : ObservableRecipient
{
    private IThemeSelectorService ThemeSelector { get; } = themeSelector;

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
        rootPage.ViewModel.Initialize();
        newWindow.Activate();
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

