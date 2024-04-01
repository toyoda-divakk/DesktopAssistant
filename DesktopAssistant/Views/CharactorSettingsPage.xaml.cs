using DesktopAssistant.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Views;

public sealed partial class CharactorSettingsPage : Page
{
    public CharactorSettingsViewModel ViewModel
    {
        get;
    }

    public CharactorSettingsPage()
    {
        ViewModel = App.GetService<CharactorSettingsViewModel>();
        InitializeComponent();
    }
}
