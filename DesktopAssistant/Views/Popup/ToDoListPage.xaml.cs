using DesktopAssistant.ViewModels.Popup;
using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Views.Popup;

public sealed partial class ToDoListPage : Page
{
    public ToDoListViewModel ViewModel
    {
        get;
    }

    public ToDoListPage()
    {
        ViewModel = App.GetService<ToDoListViewModel>();
        InitializeComponent();
    }
}
