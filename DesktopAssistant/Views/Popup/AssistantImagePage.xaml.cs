using DesktopAssistant.ViewModels.Popup;
using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Views.Popup;

public sealed partial class AssistantImagePage : Page
{
    public AssistantImageViewModel ViewModel
    {
        get;
    }

    public AssistantImagePage()
    {
        ViewModel = App.GetService<AssistantImageViewModel>();
        InitializeComponent();
    }
}
