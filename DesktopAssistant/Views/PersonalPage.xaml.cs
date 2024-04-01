using DesktopAssistant.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Views;

public sealed partial class PersonalPage : Page
{
    public PersonalViewModel ViewModel
    {
        get;
    }

    public PersonalPage()
    {
        ViewModel = App.GetService<PersonalViewModel>();
        InitializeComponent();
    }
}
