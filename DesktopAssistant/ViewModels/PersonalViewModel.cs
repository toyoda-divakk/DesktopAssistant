using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Helpers;
using DesktopAssistant.Views.Popup;

namespace DesktopAssistant.ViewModels;
// TODO:新規追加ボタンを作ろう
// TODO:コピー処理を作ろう（データ追加処理という点では↑と同じ）

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
            Source.First(x => x.Id == CurrentCharacterId).IsSelected = false;
            beforeCharacter.IsSelected = false;
            var beforeSource = Source.First(x => x.Id == CurrentCharacterId);
            beforeSource.IsSelected = false;

            var afterCharacter = _liteDbService.GetTable<Character>().First(x => x.Id == id);
            afterCharacter.IsSelected = true;
            var afterSource = Source.First(x => x.Id == id);
            afterSource.IsSelected = true;
            CurrentCharacterId = id;

            _liteDbService.Upsert(beforeCharacter); // DBに反映させる
            _liteDbService.Upsert(afterCharacter);

            // 表示更新
            var beforeIndex = Source.IndexOf(beforeSource);
            Source[beforeIndex] = beforeSource;
            var afterIndex = Source.IndexOf(afterSource);
            Source[afterIndex] = afterSource;
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        // キャラクター一覧を取得して、現在選択中のキャラクターを設定する
        var characters = _liteDbService.GetTable<Character>();
        if (!characters.Any())
        {
            return;
        }
        foreach (var character in characters)
        {
            SetupCaracter(character);
            character.Topics = _liteDbService.GetTable<Topic>().Where(x => x.CharacterId == character.Id).ToList();
            Source.Add(character);
        }
        var selectedCharacter = characters.FirstOrDefault(x => x.IsSelected);
        if (selectedCharacter == null)
        {
            var chara = characters.First();
            chara.IsSelected = true;
            _liteDbService.Upsert(chara);
            Source.First(x => x.Id == chara.Id).IsSelected = true;
            CurrentCharacterId = chara.Id;
        }
        else
        {
            CurrentCharacterId = selectedCharacter.Id;
        }

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
        // TODO:CopyCommandを作る
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
