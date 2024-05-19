using CommunityToolkit.WinUI.UI.Animations;

using DesktopAssistant.Contracts.Services;
using DesktopAssistant.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace DesktopAssistant.Views;

public sealed partial class PersonalDetailPage : Page
{
    public PersonalDetailViewModel ViewModel
    {
        get;
    }

    public PersonalDetailPage()
    {
        ViewModel = App.GetService<PersonalDetailViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        // アニメーションのやり方
        // 遷移元：
        // animations: Connected.ListItemKey = "animationKeyContentGrid"のように指定したキーを指定することで、アニメーションを適用することができる。
        // リストのうちどれか1つを選択するような時、Connected.ListItemElementName="itemThumbnail"も指定する。
        // そして、リストの要素1つを表示するGrid等にx:Name="itemThumbnail"と指定することで、その領域にアニメーションを適用するみたい。
        // 遷移先：
        // x:Name="itemHero"と指定することで、その領域にアニメーションを適用するみたい。
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
