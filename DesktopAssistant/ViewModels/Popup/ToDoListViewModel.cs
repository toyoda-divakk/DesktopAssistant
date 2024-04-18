using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Services;
using Humanizer;
using Windows.Storage;

namespace DesktopAssistant.ViewModels.Popup;

public partial class ToDoListViewModel : ObservableRecipient, INavigationAware
{
    private readonly ILiteDbService _liteDbService;

    public ObservableCollection<TodoTask> Source { get; } = [];

    public ToDoListViewModel(ILiteDbService liteDbService)
    {
        _liteDbService = liteDbService;
        Initialize();
    }


    private void Initialize()
    {
        Source.Clear();

        var data = _liteDbService.GetTable<TodoTask>();
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        // ポップアップなので呼ばれない
        Initialize();
    }

    public void OnNavigatedFrom()
    {
    }
}
