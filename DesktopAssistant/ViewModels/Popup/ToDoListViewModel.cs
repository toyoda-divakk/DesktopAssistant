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

// Navigationは使わない
// 左にカテゴリリスト、右にToDoリストを表示する
// カテゴリリストをクリックすると、ToDoリストがそのカテゴリに絞られる
// カテゴリリストを右クリックすると、カテゴリの追加・削除ダイアログが表示される
// カテゴリリストの一番上に「全て」カテゴリを表示する
// ToDoリストのアイテムをクリックすると、折り畳みが展開して詳細が表示される
// 編集画面で保存すると、ToDoリストが更新される
// ToDoリストのアイテムを右クリックすると、削除確認ダイアログが表示される（完了とは違う）
// 削除確認ダイアログで削除すると、ToDoリストが更新される
// ToDoリストのアイテムをドラッグアンドドロップすると、順番が変わる（表示順を持たせる：カテゴリ、タスク）


public partial class ToDoListViewModel(ILiteDbService liteDbService) : ObservableRecipient
{
    //private readonly ILiteDbService _liteDbService;

    public ObservableCollection<TodoTask> Source { get; } = new ObservableCollection<TodoTask>(liteDbService.GetTable<TodoTask>());
    //public ObservableCollection<TodoTask> Source { get; } = [];

    //public ToDoListViewModel(ILiteDbService liteDbService)
    //{
    //    _liteDbService = liteDbService;
    //    Initialize();
    //}

    //private void Initialize()
    //{
    //    Source.Clear();

    //    var data = _liteDbService.GetTable<TodoTask>().ToList();
    //    foreach (var item in data)
    //    {
    //        Source.Add(item);
    //    }
    //}
}
