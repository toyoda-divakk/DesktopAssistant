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
// TODO:アシスタントの選択ボタンが、選択されている状態で押すと色が戻るのを修正すべき
// TODO:アシスタントのコピー・追加・削除はテストしてないし、コピーと追加は同じような処理があるので共通化すべき

/// <summary>
/// アシスタント選択画面のViewModel
/// </summary>
public partial class PersonalViewModel(INavigationService navigationService, ILiteDbService liteDbService) : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService = navigationService;
    private readonly ILiteDbService _liteDbService = liteDbService;

    public ObservableCollection<Assistant> Source { get; } = [];

    public long CurrentAssistantId
    {
        get; set;
    }

    private void SwitchAssistant(long id)
    {
        // 設定に反映させる
        if (CurrentAssistantId != id)
        {
            var beforeAssistant = _liteDbService.GetTable<Assistant>().First(x => x.Id == CurrentAssistantId);
            Source.First(x => x.Id == CurrentAssistantId).IsSelected = false;
            beforeAssistant.IsSelected = false;
            var beforeSource = Source.First(x => x.Id == CurrentAssistantId);
            beforeSource.IsSelected = false;

            var afterAssistant = _liteDbService.GetTable<Assistant>().First(x => x.Id == id);
            afterAssistant.IsSelected = true;
            var afterSource = Source.First(x => x.Id == id);
            afterSource.IsSelected = true;
            CurrentAssistantId = id;

            _liteDbService.Upsert(beforeAssistant); // DBに反映させる
            _liteDbService.Upsert(afterAssistant);

            // 表示更新
            var beforeIndex = Source.IndexOf(beforeSource);
            Source[beforeIndex] = beforeSource;
            var afterIndex = Source.IndexOf(afterSource);
            Source[afterIndex] = afterSource;
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        // アシスタント一覧を取得して、現在選択中のアシスタントを設定する
        var assistants = _liteDbService.GetTable<Assistant>();
        if (!assistants.Any())
        {
            return;
        }
        foreach (var character in assistants)
        {
            SetupCaracter(character);
            character.Topics = _liteDbService.GetTable<Topic>().Where(x => x.AssistantId == character.Id).ToList();
            Source.Add(character);
        }
        var selectedAssistant = assistants.FirstOrDefault(x => x.IsSelected);
        if (selectedAssistant == null)
        {
            var chara = assistants.First();
            chara.IsSelected = true;
            _liteDbService.Upsert(chara);
            Source.First(x => x.Id == chara.Id).IsSelected = true;
            CurrentAssistantId = chara.Id;
        }
        else
        {
            CurrentAssistantId = selectedAssistant.Id;
        }

    }

    /// <summary>
    /// アシスタントに対して右クリック時の処理を設定する
    /// </summary>
    /// <param name="assistant"></param>
    /// <returns></returns>
    private void SetupCaracter(Assistant assistant)
    {
        assistant.EditCommand = new RelayCommand(() =>
        {
            // TODO:編集ページ遷移
        });
        assistant.CopyCommand = new RelayCommand(() =>
        {
            // このアシスタントをコピーしてDBに新規追加する
            var newAssistant = new Assistant()
            {
                Name = assistant.Name,
                Description = assistant.Description,
                Prompt = assistant.Prompt,
                BackColor = assistant.BackColor,
                TextColor = assistant.TextColor,
                IsSelected = false,
                Order = (int)_liteDbService.GetLastId<Assistant>() + 1    // 現在のID+1を取得する
            };
            var newId = newAssistant.Order; // Orderと同じ番号を採番する前提とする
            newAssistant.FaceImagePath = Path.Combine(Path.GetDirectoryName(assistant.FaceImagePath)!, $"{newId}.png");

            // DB更新
            _liteDbService.Upsert(newAssistant);
            newAssistant = _liteDbService.GetTable<Assistant>().Last(x => x.Order == newAssistant.Order);   // 採番したので取り直す

            // 画像をコピー
            File.Copy(assistant.FaceImagePath, newAssistant.FaceImagePath);

            // 画面に追加する
            Source.Add(newAssistant);

            // 右クリック処理の設定
            SetupCaracter(newAssistant);
        });
        assistant.DeleteCommand = new RelayCommand(async () =>
        {
            // 現在のWindowを取得する
            var window = App.MainWindow;

            // 削除確認ダイアログを表示する
            var contentDialogContent = new ContentDialogContent("Dialog_DeleteTask1".GetLocalized(), "Dialog_DeleteTask2".GetLocalized());      // TODO:メッセージを直すこと
            if (await DialogHelper.ShowDeleteDialog(window, "Message_DeleteCategory".GetLocalized(), contentDialogContent))
            {
                DeleteCharactor(assistant);
            }
        });
        assistant.SwitchCommand = new RelayCommand(() =>
        {
            SwitchAssistant(assistant.Id);
        });
    }

    /// <summary>
    /// DBと画面表示からアシスタント削除
    /// </summary>
    /// <param name="assistant"></param>
    private void DeleteCharactor(Assistant assistant)
    {
        // アシスタントが残り1剣の場合は削除しない
        if (Source.Count == 1)
        {
            // ダイアログを表示する
            DialogHelper.ShowMessageDialog(App.MainWindow, "Message_Error".GetLocalized(), "Message_CannotDeleteLastAssistant".GetLocalized());
            return;
        }

        // アシスタント内の会話を全て削除する
        var topics = _liteDbService.GetTable<Topic>().Where(x => x.AssistantId == assistant.Id);
        foreach (var topic in topics)
        {
            _liteDbService.Delete(topic);
        }
        // アシスタントの画像を削除する
        File.Delete(assistant.FaceImagePath);

        // アシスタントを削除する
        _liteDbService.Delete(assistant);
        Source.Remove(assistant);
    }

    public void OnNavigatedFrom()
    {
        // 画面から遷移する直前に呼び出される
        // 遷移元の画面が破棄される前に必要な後処理を実行する（データを保存するなど）
    }

    /// <summary>
    /// アシスタントをクリックした際の処理
    /// </summary>
    /// <param name="clickedItem"></param>
    [RelayCommand]
    private void OnItemClick(Assistant? clickedItem)
    {
        if (clickedItem != null)
        {
            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);       // 次のフレームナビゲーション中に使用されるオブジェクトを設定（よくわからん）
            _navigationService.NavigateTo(typeof(PersonalDetailViewModel).FullName!, clickedItem.Id);   // 行先とパラメータを指定して遷移する
        }
    }


    // アシスタント新規追加コマンド
    // 空のアシスタントデータを作成してDBに追加する
    [RelayCommand]
    private void AddNewAssistant()
    {
        var firstAssistant = _liteDbService.GetTable<Assistant>().First();
        var newId = (int)_liteDbService.GetLastId<Assistant>() + 1;    // 現在のID+1を取得する

        var newAssistant = new Assistant()
        {
            Name = "New Assistant",
            Description = "New Description",
            Prompt = "New Prompt",
            BackColor = "#FF000000",
            TextColor = "#FFFFFFFF",
            FaceImagePath = Path.Combine(Path.GetDirectoryName(firstAssistant.FaceImagePath)!, $"{newId}.png"),
            IsSelected = false,
            Order = newId
        };
        _liteDbService.Upsert(newAssistant);
        newAssistant = _liteDbService.GetTable<Assistant>().Last(x => x.Order == newAssistant.Order);
        File.Copy(firstAssistant.FaceImagePath, newAssistant.FaceImagePath);
        Source.Add(newAssistant);
        SetupCaracter(newAssistant);
    }
}
