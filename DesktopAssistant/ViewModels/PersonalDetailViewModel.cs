﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.ViewModels;

// 編集用の画面を別に作成する→GPT4-oに作ってもらおうか。

// この画面は、アシスタントの詳細を表示する画面
// 要素
// ・戻るボタン：前の画面に遷移
// ・編集ボタン：編集画面(PersonalEdit)に遷移
// ・削除ボタン：削除して前の画面に遷移
// ・アシスタント名
// ・アシスタントの画像
// ・アシスタントの説明
// ・アシスタントのプロンプト



public partial class PersonalDetailViewModel(INavigationService navigationService, ILiteDbService liteDbService) : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService = navigationService;
    private readonly ILiteDbService _liteDbService = liteDbService;

    [ObservableProperty]
    private Assistant? item;

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is long assistantId)
        {
            Item = _liteDbService.GetTable<Assistant>().First(x => x.Id == assistantId);
        }
    }

    [RelayCommand]
    public void GoBack()
    {
        // PersonalPageに遷移する
        _navigationService.GoBack();
    }

    [RelayCommand]
    public void Edit()
    {
        // 編集画面に遷移する処理を実装する
        _navigationService.NavigateTo(typeof(PersonalEditViewModel).FullName!, Item!.Id);
    }

    [RelayCommand]
    public void Delete()
    {
        // 削除して前の画面に遷移する処理を実装する
        // TODO: 削除確認ダイアログを表示する
        _liteDbService.Delete<Assistant>(Item!);
        _navigationService.GoBack();
    }

    public void OnNavigatedFrom()
    {
        // 画面から遷移する直前に呼び出される
        // 遷移元の画面が破棄される前に必要な後処理を実行する（データを保存するなど）
    }
}