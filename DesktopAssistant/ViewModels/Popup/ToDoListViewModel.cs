﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Helpers;
using DesktopAssistant.Services;
using DesktopAssistant.Views.Popup;
using Humanizer;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace DesktopAssistant.ViewModels.Popup;

// TODO:カテゴリリストを右クリックすると、カテゴリの追加・削除ダイアログが表示される
// TODO:ToDoリストのアイテムをクリックすると、折り畳みが展開して詳細が表示される
// TODO:編集画面で保存すると、ToDoリストが更新される
// TODO:ToDoリストのアイテムを右クリックすると、削除確認ダイアログが表示される（完了とは違う）
// TODO:削除確認ダイアログで削除すると、ToDoリストが更新される
// TODO:ToDoリストのアイテムをドラッグアンドドロップすると、順番が変わる（表示順を持たせる：カテゴリ、タスク）

// WinUI3 GalleryのListViewを参考に実装する

public partial class ToDoListViewModel(ILiteDbService liteDbService, IDateUtilService dateUtilService) : ObservableRecipient
{
    private readonly ILiteDbService _liteDbService = liteDbService;

    /// <summary>
    /// カテゴリリスト
    /// </summary>
    public ObservableCollection<TaskCategory> Categories { get; set; } = [];

    /// <summary>
    /// ウィンドウの参照
    /// モーダルダイアログを出すのに必要
    /// </summary>
    private Window _window { get; set; } = null!;

    /// <summary>
    /// 表示中のタスク
    /// </summary>
    public ObservableCollection<TodoTask> Tasks { get; set; } = [];

    /// <summary>
    /// 初期化
    /// カテゴリのリストを読み込む
    /// カテゴリ内のタスクを読み込む
    /// </summary>
    public void Initialize(Window window)
    {
        var taskCategories = _liteDbService.GetTable<TaskCategory>();
        foreach (var category in taskCategories)
        {
            SetupCategory(category);
            category.TodoTasks = _liteDbService.GetTable<TodoTask>().Where(x => x.CategoryId == category.Id).ToList();
            Categories.Add(category);
        }
        _window = window;
    }

    /// <summary>
    /// カテゴリに対して右クリック時の処理を設定する
    /// </summary>
    /// <param name="taskCategories"></param>
    /// <returns></returns>
    private void SetupCategory(TaskCategory category)
    {
        category.EditCommand = new RelayCommand(() =>
        {
            // TODO:編集ダイアログを表示する→遷移できるならそっちの方が良いなあWindowにFrameを持たせるとか（たぶんできない。それは確認すること。）
        });
        category.DeleteCommand = new RelayCommand<Action>(async (Action? func) =>
        {
            // 削除確認ダイアログを表示する
            var contentDialogContent = new ContentDialogContent("Dialog_DeleteTask1".GetLocalized(), "Dialog_DeleteTask2".GetLocalized());
            if (await DialogHelper.ShowDeleteDialog(_window!, "Message_DeleteCategory".GetLocalized(), contentDialogContent))
            {
                DeleteCategory(category);
            }
        });
    }

    /// <summary>
    /// DBと画面表示からカテゴリ削除
    /// </summary>
    /// <param name="category"></param>
    private void DeleteCategory(TaskCategory category)
    {
        // カテゴリ内のタスクを全て削除する
        var tasks = _liteDbService.GetTable<TodoTask>().Where(x => x.CategoryId == category.Id);
        foreach (var task in tasks)
        {
            _liteDbService.Delete(task);
        }
        // カテゴリを削除する
        _liteDbService.Delete(category);
        Categories.Remove(category);
        SetTasks([]);
    }

    /// <summary>
    /// カテゴリ切り替え
    /// </summary>
    /// <param name="category"></param>
    [RelayCommand]
    public void TaskCategoryChanged(object category)
    {
        var stringItem = category as TaskCategory;
        if (stringItem == null)
        {
            return;
        }
        SetTasks(stringItem.TodoTasks);
    }

    /// <summary>
    /// 全てのカテゴリのタスクを表示する
    /// </summary>
    [RelayCommand]
    public void ShowAllTasks()
    {
        var tasks = _liteDbService.GetTable<TodoTask>();
        foreach (var todoTask in tasks)
        {
            Tasks.Add(todoTask);
        }
        SetTasks(tasks);
    }

    [RelayCommand]
    public void AddCategory()
    {
    }

    /// <summary>
    /// タスクの表示を更新する
    /// </summary>
    /// <param name="tasks">表示するタスクのリスト</param>
    private void SetTasks(IEnumerable<TodoTask> tasks)
    {
        Tasks.Clear();
        foreach (var todoTask in tasks)
        {
            Tasks.Add(todoTask);
        }
    }
}

