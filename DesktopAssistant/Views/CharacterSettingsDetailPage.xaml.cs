using CommunityToolkit.WinUI.UI.Animations;

using DesktopAssistant.Contracts.Services;
using DesktopAssistant.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace DesktopAssistant.Views;

public sealed partial class CharacterSettingsDetailPage : Page
{
    public CharacterSettingsDetailViewModel ViewModel
    {
        get;
    }

    public CharacterSettingsDetailPage()
    {
        ViewModel = App.GetService<CharacterSettingsDetailViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.NavigationMode == NavigationMode.Back)
        {
            var navigationService = App.GetService<INavigationService>();

            if (ViewModel.Item != null)
            {
                navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
