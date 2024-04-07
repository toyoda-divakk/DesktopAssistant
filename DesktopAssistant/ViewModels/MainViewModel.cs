using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Helpers;
using DesktopAssistant.Views.Popup;

namespace DesktopAssistant.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    // ToDoListPageをモードレス表示するコマンドを作成する
    [RelayCommand]
    private void OnShowToDoList()
    {
        var newWindow = WindowHelper.CreateWindow();
        var rootPage = new ToDoListPage();
        //rootPage.RequestedTheme = ThemeHelper.RootTheme;
        newWindow.Content = rootPage;
        newWindow.Activate();

        //var targetPageType = typeof(HomePage);
        //string targetPageArguments = string.Empty;
        //rootPage.Navigate(targetPageType, targetPageArguments);
    }

}

