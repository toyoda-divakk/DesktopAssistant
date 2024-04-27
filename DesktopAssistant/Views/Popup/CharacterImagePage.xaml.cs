using DesktopAssistant.ViewModels.Popup;
using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Views.Popup;

public sealed partial class CharacterImagePage : Page
{
    public CharacterImageViewModel ViewModel
    {
        get;
    }

    public CharacterImagePage()
    {
        ViewModel = App.GetService<CharacterImageViewModel>();
        InitializeComponent();
    }
}
