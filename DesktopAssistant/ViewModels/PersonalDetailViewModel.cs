using CommunityToolkit.Mvvm.ComponentModel;

using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.ViewModels;

public partial class PersonalDetailViewModel(ISampleDataService sampleDataService) : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private Character? item;

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is long orderID)
        {
            var data = sampleDataService.GetSampleCharacters();    // TODO:データベースからの取得に変更
            Item = data.First(i => i.Id == orderID);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
