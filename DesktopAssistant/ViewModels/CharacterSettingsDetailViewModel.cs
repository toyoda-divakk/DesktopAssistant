using CommunityToolkit.Mvvm.ComponentModel;

using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.ViewModels;

public partial class CharacterSettingsDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    [ObservableProperty]
    private Character? item;

    public CharacterSettingsDetailViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is long characterID)
        {
            var data = _sampleDataService.GetSampleCharacters();    // TODO:データベースからの取得に変更
            Item = data.First(i => i.Id == characterID);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
