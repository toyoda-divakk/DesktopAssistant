﻿using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.ViewModels;

public partial class CharacterSettingsViewModel(INavigationService navigationService, ISampleDataService sampleDataService) : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService = navigationService;
    private readonly ISampleDataService _sampleDataService = sampleDataService;

    public ObservableCollection<Character> Source { get; } = new();

    public void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = _sampleDataService.GetSampleCharacters();    // TODO:データベースからの取得に変更
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void OnItemClick(Character? clickedItem)
    {
        if (clickedItem != null)
        {
            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
            _navigationService.NavigateTo(typeof(CharacterSettingsDetailViewModel).FullName!, clickedItem.Id);
        }
    }
}
