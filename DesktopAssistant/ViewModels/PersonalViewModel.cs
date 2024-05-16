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
public partial class PersonalViewModel(INavigationService navigationService, ILiteDbService liteDbService) : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService = navigationService;
    private readonly ILiteDbService _liteDbService = liteDbService;

    public ObservableCollection<Character> Source { get; } = [];

    public long CurrentCharacterId
    {
        get; set;
    }

    private void SwitchCharacter(long id)
    {
        // 設定に反映させる
        if (CurrentCharacterId != id)
        {
            var beforeCharacter = _liteDbService.GetTable<Character>().First(x => x.Id == CurrentCharacterId);
            beforeCharacter.IsSelected = false;
            var afterCharacter = _liteDbService.GetTable<Character>().First(x => x.Id == id);
            afterCharacter.IsSelected = true;
            CurrentCharacterId = id;

            // DBに反映させる（変更前と変更後の2件）
            _liteDbService.Upsert(beforeCharacter);
            _liteDbService.Upsert(afterCharacter);
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        // キャラクター一覧を取得して、現在選択中のキャラクターを設定する
        var characters = _liteDbService.GetTable<Character>();
        foreach (var character in characters)
        {
            SetupCaracter(character);
            character.Topics = _liteDbService.GetTable<Topic>().Where(x => x.CharacterId == character.Id).ToList();
            Source.Add(character);
        }
        CurrentCharacterId = Source.First(x => x.IsSelected).Id;
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
            // TODO:編集ダイアログを表示する→遷移できるならそっちの方が良いなあ
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
        // 画面から遷移する直前に呼び出される
        // 遷移元の画面が破棄される前に必要な後処理を実行する（データを保存するなど）
    }

    /// <summary>
    /// キャラクターをクリックした際の処理
    /// </summary>
    /// <param name="clickedItem"></param>
    [RelayCommand]
    private void OnItemClick(Character? clickedItem)
    {
        if (clickedItem != null)
        {
            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);       // 次のフレームナビゲーション中に使用されるオブジェクトを設定（よくわからん）
            _navigationService.NavigateTo(typeof(PersonalDetailViewModel).FullName!, clickedItem.Id);   // 行先とパラメータを指定して遷移する
        }
    }
}
