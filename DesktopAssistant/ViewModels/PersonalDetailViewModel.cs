using CommunityToolkit.Mvvm.ComponentModel;

using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.ViewModels;

public partial class PersonalDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    [ObservableProperty]
    private Character? item;

    public PersonalDetailViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is long orderID)
        {
            var data = _sampleDataService.GetSampleCharacters();    // TODO:データベースからの取得に変更
            Item = data.First(i => i.Id == orderID);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
