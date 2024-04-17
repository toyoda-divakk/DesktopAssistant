using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Services;

namespace DesktopAssistant.ViewModels.Popup;

public partial class ToDoListViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private readonly ILiteDbService _liteDbService;
    private readonly ILocalSettingsService _localSettingsService;

    public ObservableCollection<TodoTask> Source { get; } = [];

    public ToDoListViewModel(ISampleDataService sampleDataService, ILiteDbService liteDbService, ILocalSettingsService localSettingsService)
    {
        _sampleDataService = sampleDataService;
        _liteDbService = liteDbService;
        _localSettingsService = localSettingsService;
        Initialize();
    }

    //private async void InitializeAsync()
    //{
    //    Source.Clear();

    //    var data = await _sampleDataService.GetTodoTaskDataAsync();

    //    foreach (var item in data)
    //    {
    //        Source.Add(item);
    //    }
    //}

    private void Initialize()
    {
        Source.Clear();

        //var data = await _sampleDataService.GetTodoTaskDataAsync();
        var data = _liteDbService.Test(_localSettingsService.GetApplicationDataFolder());

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        Initialize();
    }

    public void OnNavigatedFrom()
    {
    }
}
