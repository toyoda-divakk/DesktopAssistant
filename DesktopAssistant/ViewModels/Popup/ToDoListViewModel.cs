using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.ViewModels.Popup;

public partial class ToDoListViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<TodoTask> Source { get; } = new ObservableCollection<TodoTask>();

    public ToDoListViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = await _sampleDataService.GetTodoTaskDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
