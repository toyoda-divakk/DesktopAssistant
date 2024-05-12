using System.IO;
using System.Reflection;
using DesktopAssistant.Activation;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Core.Services;
using DesktopAssistant.Views;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace DesktopAssistant.Services;

// ActivationHandlerを使用することで、アプリケーションの起動時に特定のアクティベーション引数を処理するための柔軟性と拡張性を実現することができます。
/// <summary>
/// アプリケーションの起動時に実行されるさまざまなタスクを処理する
/// アプリケーションの初期化
/// アクティベーションハンドラの処理
/// テーマの設定など
/// </summary>
public class ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, 
    IEnumerable<IActivationHandler> activationHandlers,
    IThemeSelectorService themeSelectorService,
    IApiSettingService apiSettingService,
    IChatSettingService chatSettingService,
    ICharacterSettingService characterSettingService,
    ILiteDbService liteDbService,
    ISampleDataService sampleDataService,
    ILocalSettingsService localSettingsService) : IActivationService
{
    private UIElement? _shell = null;
    //private UIElement? _main = null;

    /// <summary>
    /// アプリケーションの起動時に呼び出される
    /// </summary>
    /// <param name="activationArgs">アクティベーション引数</param>
    /// <returns></returns>
    public async Task ActivateAsync(object activationArgs)
    {
        // アクティベーション前のタスク
        await InitializeAsync();

        // MainWindowのコンテンツの設定
        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<ShellPage>();
            App.MainWindow.Content = _shell ?? new Frame();
            //_main = App.GetService<MainPage>();
            //App.MainWindow.Content = _main ?? new Frame();
        }

        await HandleActivationAsync(activationArgs);

        // MainWindowのアクティベーション
        App.MainWindow.Activate();

        // StartupAsyncメソッドの呼び出し
        await StartupAsync();
    }

    /// <summary>
    /// アクティベーション引数を処理
    /// 与えられたアクティベーション引数に対して、登録されたActivationHandlerの中から適切なハンドラを見つけて実行します。
    /// </summary>
    /// <param name="activationArgs">アクティベーション引数</param>
    /// <returns></returns>
    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (defaultHandler.CanHandle(activationArgs))
        {
            await defaultHandler.HandleAsync(activationArgs);
        }
    }

    /// <summary>
    /// 最初に実行
    /// テーマの初期化など
    /// </summary>
    /// <returns></returns>
    private async Task InitializeAsync()
    {
        // 設定ファイルを初期化
        await themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await apiSettingService.InitializeAsync().ConfigureAwait(false);
        await chatSettingService.InitializeAsync().ConfigureAwait(false);
        await characterSettingService.InitializeAsync().ConfigureAwait(false);

        // 初回起動限定タスクの実行
        if (!liteDbService.IsExistDatabase())
        {
            liteDbService.CreateOrInitializeDatabase();
        }
        // 1度しか実行しない処理が行われているかここでチェックし、実施されていない場合は実行する。SystemEventが登録されていれば実行済み。
        var systemEvents = liteDbService.GetTable<SystemEvent>().ToList();
        if (!systemEvents.Exists(e => e.Event == SystemEvents.Initial_SetTasks))
        {
            liteDbService.Insert(new SystemEvent()
            {
                Event = SystemEvents.Initial_SetTasks,
                Content = "DBにプリセットのタスクを登録する。",
                IsDone = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            // サンプルデータを登録
            var taskCategories = sampleDataService.GetSampleTodoTasks();
            foreach (var taskCategory in taskCategories)
            {
                liteDbService.Insert(taskCategory);
                foreach (var task in taskCategory.TodoTasks)
                {
                    liteDbService.Insert(task);
                }
            }
        }
        if (!systemEvents.ToList().Exists(e => e.Event == SystemEvents.Initial_SetCharacoers))
        {
            liteDbService.Insert(new SystemEvent()
            {
                Event = SystemEvents.Initial_SetCharacoers,
                Content = "DBにプリセットのキャラを登録する。",
                IsDone = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            // サンプルデータを登録
            var characters = sampleDataService.GetSampleCharacters();
            foreach (var character in characters)
            {
                character.FaceImagePath = Path.Combine(GetImageFolder(), $"{character.Id.ToString()}.png");
                liteDbService.Insert(character);
                foreach (var topic in character.Topics)
                {
                    liteDbService.Insert(topic);
                }
            }

            // 埋め込みリソースからサンプル画像を取得してアプリケーションフォルダにコピーする
            CreateImageFolderIfNotExists();
            CopyResorceImageToDataFolder("DesktopAssistant.SampleImages.devil.png", "Images/1.png");
            CopyResorceImageToDataFolder("DesktopAssistant.SampleImages.ookami.png", "Images/2.png");
            CopyResorceImageToDataFolder("DesktopAssistant.SampleImages.slime.png", "Images/3.png");
            CopyResorceImageToDataFolder("DesktopAssistant.SampleImages.tsundere.png", "Images/4.png");
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// 画像フォルダのパスを取得する
    /// </summary>
    private string GetImageFolder()
    {
        var appFolder = localSettingsService.ApplicationDataFolder;
        var imageFolder = Path.Combine(appFolder, "Images");
        return imageFolder;
    }

    /// <summary>
    /// 画像フォルダが無ければ作成する
    /// </summary>
    private void CreateImageFolderIfNotExists()
    {
        // アプリケーションフォルダのパスを取得  
        var imageFolder = GetImageFolder();
        if (!Directory.Exists(imageFolder))
        {
            Directory.CreateDirectory(imageFolder);
        }
    }

    /// <summary>
    /// 埋め込まれたリソースをデータフォルダにコピーする
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    private void CopyResorceImageToDataFolder(string source, string destination)
    {
        // アプリケーションフォルダのパスを取得  
        var appFolder = localSettingsService.ApplicationDataFolder;
        // 埋め込まれたリソースから画像ファイルを取得  
        var assembly = typeof(MainPage).GetTypeInfo().Assembly;
        using var resourceStream = assembly.GetManifestResourceStream(source);
        using var fileStream = File.Create(Path.Combine(appFolder, destination));
        resourceStream?.CopyTo(fileStream);
    }

    /// <summary>
    /// 最後に実行
    /// </summary>
    /// <returns></returns>
    private async Task StartupAsync()
    {
        await themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
    }
}
