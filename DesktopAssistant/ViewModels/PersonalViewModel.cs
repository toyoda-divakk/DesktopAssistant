using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Helpers;
using DesktopAssistant.Services;
using DesktopAssistant.Views.Popup;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.ViewModels;

/// <summary>
/// キャラ選択画面のViewModel
/// </summary>
public partial class PersonalViewModel(INavigationService navigationService, ILiteDbService liteDbService, ICharacterSettingService characterSettingService) : ObservableRecipient, INavigationAware, ICharacterSetting
{
    private readonly INavigationService _navigationService = navigationService;
    private readonly ILiteDbService _liteDbService = liteDbService;
    private readonly ICharacterSettingService _characterSettingService = characterSettingService;

    public ObservableCollection<Character> Source { get; } = [];

    public Character CurrentCharacter => Source.First(x => x.Id == CurrentCharacterId);
    public long CurrentCharacterId
    {
        get; set;
    }

    private void SwitchCharacter(long id)
    {
        // 表示に反映させる
        foreach (var character in Source)
        {
            character.IsSelected = character.Id == id;
        }
        // 設定に反映させる
        if (CurrentCharacterId != id)
        {
            CurrentCharacterId = id;
            _characterSettingService.SetAndSaveAsync(this);
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        var characters = _liteDbService.GetTable<Character>();
        foreach (var character in characters)
        {
            SetupCaracter(character);
            character.Topics = _liteDbService.GetTable<Topic>().Where(x => x.CharacterId == character.Id).ToList();
            Source.Add(character);
        }

        // _characterSettingServiceから選択中のキャラクターを取得して選択状態にする
        CurrentCharacterId = _characterSettingService.CurrentCharacter.Id;
        SwitchCharacter(CurrentCharacterId);
    }

    /// <summary>
    /// キャラクターに対して右クリック時の処理を設定する
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    private void SetupCaracter(Character character)
    {
        character.EditCommand = new RelayCommand(() =>
        {
            // TODO:編集ダイアログを表示する→遷移できるならそっちの方が良いなあWindowにFrameを持たせるとか（たぶんできない。それは確認すること。）
        });
        character.DeleteCommand = new RelayCommand(async () =>
        {
            // 現在のWindowを取得する
            var window = App.MainWindow;

            // 削除確認ダイアログを表示する
            var contentDialogContent = new ContentDialogContent("Dialog_DeleteTask1".GetLocalized(), "Dialog_DeleteTask2".GetLocalized());      // TODO:メッセージを直すこと
            if (await DialogHelper.ShowDeleteDialog(window, "Message_DeleteCategory".GetLocalized(), contentDialogContent))
            {
                DeleteCharactor(character);
            }
        });
        character.SwitchCommand = new RelayCommand(() =>
        {
            SwitchCharacter(character.Id);
        });
    }

    /// <summary>
    /// DBと画面表示からキャラクター削除
    /// </summary>
    /// <param name="character"></param>
    private void DeleteCharactor(Character character)
    {
        // キャラクター内の会話を全て削除する
        var topics = _liteDbService.GetTable<Topic>().Where(x => x.CharacterId == character.Id);
        foreach (var topic in topics)
        {
            _liteDbService.Delete(topic);
        }
        // キャラクターを削除する
        _liteDbService.Delete(character);
        Source.Remove(character);
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
            _navigationService.NavigateTo(typeof(PersonalDetailViewModel).FullName!, clickedItem.Id);
        }
    }
}
