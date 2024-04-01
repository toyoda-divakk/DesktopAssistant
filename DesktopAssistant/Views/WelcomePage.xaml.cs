using DesktopAssistant.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Views;

public sealed partial class WelcomePage : Page
{
    public WelcomeViewModel ViewModel
    {
        get;
    }

    public WelcomePage()
    {
        ViewModel = App.GetService<WelcomeViewModel>();
        InitializeComponent();
    }
}
