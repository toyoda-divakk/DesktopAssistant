using System.Collections.Generic;
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

// WinUI3 GalleryのListViewを参考に実装する

public partial class ToDoListViewModel(ILiteDbService liteDbService) : ObservableRecipient
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
    private Window? _window;

    /// <summary>
    /// 表示中のタスク
    /// </summary>
    public ObservableCollection<TodoTask> Tasks { get; set; } = [];

    /// <summary>
    /// 初期化
    /// カテゴリのリストを読み込む
    /// </summary>
    public void Initialize(Window window)
    {
        var taskCategories = _liteDbService.GetTable<TaskCategory>();
        Categories = SetupCategories(taskCategories);
        _window = window;
    }

    /// <summary>
    /// 全カテゴリ取得
    /// </summary>
    /// <param name="taskCategories"></param>
    /// <returns></returns>
    private ObservableCollection<TaskCategory> SetupCategories(IEnumerable<TaskCategory> taskCategories)
    {
        var categories = taskCategories.ToList();

        // 右クリック時の処理を設定する
        foreach (var category in categories)
        {
            category.EditCommand = new RelayCommand(() =>
            {
                // TODO:編集ダイアログを表示する
            });
            category.DeleteCommand = new RelayCommand(async () =>
            {
                // TODO:削除確認ダイアログを表示する
                var dialog1 = new ContentDialog
                {
                    XamlRoot = _window?.Content.XamlRoot,
                    Title = "Message_DeleteCategory".GetLocalized(),
                    PrimaryButtonText = "Button_Delete".GetLocalized(),
                    CloseButtonText = "Button_Cancel".GetLocalized(),
                    DefaultButton = ContentDialogButton.Primary,
                    Content = new ContentDialogContent()
                };

                var result1 = await dialog1.ShowAsync();
                if (result1 == ContentDialogResult.Primary)
                {
                    var dialog2 = new ContentDialog
                    {
                        XamlRoot = _window?.Content.XamlRoot,
                        Title = "Message_ConfirmDelete1".GetLocalized(),
                        PrimaryButtonText = "Button_Delete".GetLocalized(),
                        CloseButtonText = "Button_Cancel".GetLocalized(),
                        DefaultButton = ContentDialogButton.Primary,
                        Content = new ContentDialogContent()
                    };
                    var result2 = await dialog2.ShowAsync();
                    if (result2 == ContentDialogResult.Primary)
                    {
                        var dialog3 = new ContentDialog
                        {
                            XamlRoot = _window?.Content.XamlRoot,
                            Title = "Message_ConfirmDelete2".GetLocalized(),
                            PrimaryButtonText = "Button_NoRegrets".GetLocalized(),
                            CloseButtonText = "Button_Cancel".GetLocalized(),
                            DefaultButton = ContentDialogButton.Primary,
                            Content = new ContentDialogContent()
                        };
                        var result3 = await dialog3.ShowAsync();
                        if (result3 == ContentDialogResult.Primary)
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
                    }
                }

            });
        }
        return new ObservableCollection<TaskCategory>(categories);
    }

    /// <summary>
    /// 
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

