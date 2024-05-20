using CommunityToolkit.Mvvm.ComponentModel;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Services;
namespace DesktopAssistant.ViewModels;

public partial class PersonalEditViewModel(INavigationService navigationService, ILiteDbService liteDbService) : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService = navigationService;
    private readonly ILiteDbService _liteDbService = liteDbService;

    [ObservableProperty]
    private Character? item;

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is long characterId)
        {
            Item = _liteDbService.GetTable<Character>().First(x => x.Id == characterId);
        }
    }

    public void OnNavigatedFrom()
    {
        // 画面から遷移する直前に呼び出される
        // 遷移元の画面が破棄される前に必要な後処理を実行する（データを保存するなど）
    }
}
