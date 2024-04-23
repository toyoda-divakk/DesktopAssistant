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

// WinUI3 GalleryのListViewを参考に実装する

public partial class ToDoListViewModel(ILiteDbService liteDbService) : ObservableRecipient
{
    private readonly ILiteDbService _liteDbService = liteDbService;

    /// <summary>
    /// カテゴリリスト
    /// </summary>
    public ObservableCollection<TaskCategory> Categories { get; set; } = [];

    /// <summary>
    /// 表示中のタスク
    /// </summary>
    public ObservableCollection<TodoTask> Tasks { get; set; } = [];

    ObservableCollection<TaskCategory> SetupCategories(IEnumerable<TaskCategory> taskCategories)
    {
        var allCategory = new TaskCategory
        {
            Id = 0,
            Name = "全て",
            TodoTasks = [],
            EditCommand = new RelayCommand(() => { })
        };

        var categories = taskCategories.ToList();
        foreach (var category in categories)
        {
            category.EditCommand = new RelayCommand(() =>
            {
                Tasks = new ObservableCollection<TodoTask>(category.TodoTasks);
            });
        }

        return new ObservableCollection<TaskCategory>(categories);
    }

    public void Initialize()
    {
        var taskCategories = _liteDbService.GetTable<TaskCategory>();
        Categories = SetupCategories(taskCategories);
    }

    [RelayCommand]
    public void TaskCategoryChanged(object category)
    {
        Tasks.Clear();

        var stringItem = category as TaskCategory;
        if (stringItem == null)
        {
            return;
        }
        foreach (var todoTask in stringItem.TodoTasks)
        {
            Tasks.Add(todoTask);
        }
    }
}

