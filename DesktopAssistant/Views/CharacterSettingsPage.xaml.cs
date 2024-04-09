using DesktopAssistant.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Views;

public sealed partial class CharacterSettingsPage : Page
{
    public CharacterSettingsViewModel ViewModel
    {
        get;
    }

    public CharacterSettingsPage()
    {
        ViewModel = App.GetService<CharacterSettingsViewModel>();
        InitializeComponent();
    }
}
