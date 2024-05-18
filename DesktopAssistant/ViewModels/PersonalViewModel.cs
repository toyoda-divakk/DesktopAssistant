using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Helpers;
using DesktopAssistant.Services;
using DesktopAssistant.Views.Popup;
using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.ViewModels;
// TODO:キャラクターのコピーと追加削除はテストしてないし、同じような処理がコピーと追加にあるので共通化すべき

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
        character.CopyCommand = new RelayCommand(() =>
        {
            // このキャラクターをコピーしてDBに新規追加する
            var newCharacter = new Character()
            {
                Name = character.Name,
                Description = character.Description,
                Prompt = character.Prompt,
                BackColor = character.BackColor,
                TextColor = character.TextColor,
                IsSelected = false,
                Order = (int)_liteDbService.GetLastId<Character>() + 1    // 現在のID+1を取得する
            };
            var newId = newCharacter.Order; // Orderと同じ番号を採番する前提とする
            newCharacter.FaceImagePath = Path.Combine(Path.GetDirectoryName(character.FaceImagePath)!, $"{newId}.png");

            // DB更新
            _liteDbService.Upsert(newCharacter);
            newCharacter = _liteDbService.GetTable<Character>().Last(x => x.Order == newCharacter.Order);   // 採番したので取り直す

            // 画像をコピー
            File.Copy(character.FaceImagePath, newCharacter.FaceImagePath);

            // 画面に追加する
            Source.Add(newCharacter);

            // 右クリック処理の設定
            SetupCaracter(newCharacter);
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
        // キャラクターが残り1剣の場合は削除しない
        if (Source.Count == 1)
        {
            // ダイアログを表示する
            DialogHelper.ShowMessageDialog(App.MainWindow, "Message_Error".GetLocalized(), "Message_CannotDeleteLastCharacter".GetLocalized());
            return;
        }

        // キャラクター内の会話を全て削除する
        var topics = _liteDbService.GetTable<Topic>().Where(x => x.CharacterId == character.Id);
        foreach (var topic in topics)
        {
            _liteDbService.Delete(topic);
        }
        // キャラクターの画像を削除する
        File.Delete(character.FaceImagePath);

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


    // TODO:生成したままなので要テスト
    // キャラクター新規追加コマンド
    // 空のキャラクターデータを作成してDBに追加する
    [RelayCommand]
    private void AddNewCharacter()
    {
        var firstCharacter = _liteDbService.GetTable<Character>().First();
        var newId = (int)_liteDbService.GetLastId<Character>() + 1;    // 現在のID+1を取得する

        var newCharacter = new Character()
        {
            Name = "New Character",
            Description = "New Description",
            Prompt = "New Prompt",
            BackColor = "#FF000000",
            TextColor = "#FFFFFFFF",
            FaceImagePath = Path.Combine(Path.GetDirectoryName(firstCharacter.FaceImagePath)!, $"{newId}.png"),
            IsSelected = false,
            Order = newId
        };
        _liteDbService.Upsert(newCharacter);
        newCharacter = _liteDbService.GetTable<Character>().Last(x => x.Order == newCharacter.Order);
        File.Copy(firstCharacter.FaceImagePath, newCharacter.FaceImagePath);
        Source.Add(newCharacter);
        SetupCaracter(newCharacter);
    }
}
