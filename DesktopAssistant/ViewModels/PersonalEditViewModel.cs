using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Services;
namespace DesktopAssistant.ViewModels;

// TODO:チャット画面とTodo画面のボタンをリソースごと直すこと。削除確認画面を出すこと。

public partial class PersonalEditViewModel(INavigationService navigationService, ILiteDbService liteDbService) : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService = navigationService;
    private readonly ILiteDbService _liteDbService = liteDbService;

    [ObservableProperty]
    private Assistant? item;

    /// <summary>
    /// 更新してアシスタント一覧に戻る
    /// </summary>
    [RelayCommand]
    public void Update()
    {
        _liteDbService.Upsert(Item!);
        _navigationService.NavigateTo(typeof(PersonalViewModel).FullName!);
    }

    /// <summary>
    /// 一覧に戻る
    /// </summary>
    [RelayCommand]
    public void BackToList()
    {
        _navigationService.NavigateTo(typeof(PersonalViewModel).FullName!);
    }

    /// <summary>
    /// 前に戻る（詳細画面）
    /// </summary>
    [RelayCommand]
    public void GoBack()
    {
        _navigationService.GoBack();
    }

    /// <summary>
    /// 削除して一覧に戻る
    /// </summary>
    [RelayCommand]
    public void Delete()
    {
        _navigationService.NavigateTo(typeof(PersonalViewModel).FullName!);
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is long assistantId)
        {
            Item = _liteDbService.GetAssistants().First(x => x.Id == assistantId);
        }
    }

    public void OnNavigatedFrom()
    {
        // 画面から遷移する直前に呼び出される
        // 遷移元の画面が破棄される前に必要な後処理を実行する（データを保存するなど）
    }
}
