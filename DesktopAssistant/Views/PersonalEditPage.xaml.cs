using DesktopAssistant.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Views;

public sealed partial class PersonalEditPage : Page
{
    public PersonalEditViewModel ViewModel
    {
        get;
    }

    public PersonalEditPage()
    {
        ViewModel = App.GetService<PersonalEditViewModel>();
        InitializeComponent();
    }
}
