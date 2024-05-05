using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Helpers;
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

    public void OnNavigatedTo(object parameter)
    {
        var characters = _liteDbService.GetTable<Character>();
        foreach (var character in characters)
        {
            SetupCaracter(character);
            character.Topics = _liteDbService.GetTable<Topic>().Where(x => x.CharacterId == character.Id).ToList();
            Source.Add(character);
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
            // TODO:編集ダイアログを表示する→遷移できるならそっちの方が良いなあWindowにFrameを持たせるとか（たぶんできない。それは確認すること。）
        });
        character.DeleteCommand = new RelayCommand(async () =>
        {
            //// 削除確認ダイアログを表示する
            //// TODO:結構かさばるから共通化したい。
            //var dialog1 = new ContentDialog
            //{
            //    XamlRoot = _window?.Content.XamlRoot,
            //    Title = "Message_DeleteCategory".GetLocalized(),
            //    PrimaryButtonText = "Button_Delete".GetLocalized(),
            //    CloseButtonText = "Button_Cancel".GetLocalized(),
            //    DefaultButton = ContentDialogButton.Primary,
            //    Content = new ContentDialogContent("Dialog_DeleteTask1".GetLocalized(), "Dialog_DeleteTask2".GetLocalized())
            //};

            //var result1 = await dialog1.ShowAsync();
            //if (result1 == ContentDialogResult.Primary)
            //{
            //    var dialog2 = new ContentDialog
            //    {
            //        XamlRoot = _window?.Content.XamlRoot,
            //        Title = "Message_ConfirmDelete1".GetLocalized(),
            //        PrimaryButtonText = "Button_Delete".GetLocalized(),
            //        CloseButtonText = "Button_Cancel".GetLocalized(),
            //        DefaultButton = ContentDialogButton.Primary,
            //        Content = new ContentDialogContent("Dialog_DeleteTask1".GetLocalized(), "Dialog_DeleteTask2".GetLocalized())
            //    };
            //    var result2 = await dialog2.ShowAsync();
            //    if (result2 == ContentDialogResult.Primary)
            //    {
            //        var dialog3 = new ContentDialog
            //        {
            //            XamlRoot = _window?.Content.XamlRoot,
            //            Title = "Message_ConfirmDelete2".GetLocalized(),
            //            PrimaryButtonText = "Button_NoRegrets".GetLocalized(),
            //            CloseButtonText = "Button_Cancel".GetLocalized(),
            //            DefaultButton = ContentDialogButton.Primary,
            //            Content = new ContentDialogContent("Dialog_DeleteTask1".GetLocalized(), "Dialog_DeleteTask2".GetLocalized())
            //        };
            //        var result3 = await dialog3.ShowAsync();
            //        if (result3 == ContentDialogResult.Primary)
            //        {
            //            DeleteCharactor(character);

            //        }
            //    }
            //}
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
