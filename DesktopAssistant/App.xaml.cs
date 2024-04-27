using DesktopAssistant.Activation;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Services;
using DesktopAssistant.Helpers;
using DesktopAssistant.Models;
using DesktopAssistant.Services;
using DesktopAssistant.ViewModels;
using DesktopAssistant.ViewModels.Popup;
using DesktopAssistant.Views;
using DesktopAssistant.Views.Popup;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace DesktopAssistant;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} はApp.xaml.cs内のConfigureServicesに登録する必要がある。");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // ActivationHandlerを登録して、起動時の処理を設定する
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>(); // 実行条件と実行内容を設定したら、起動時にやってくれる

            // 他に Activation Handlers があればここに追加する

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();   // 設定内容を保存
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();   // テーマの変更と保存
            services.AddSingleton<IApiSettingService, ApiSettingService>();         // APIの変更と保存
            services.AddTransient<INavigationViewService, NavigationViewService>(); // NavigationViewの操作を補助する

            services.AddSingleton<IActivationService, ActivationService>();     // アプリケーション起動時の処理を行う
            services.AddSingleton<IPageService, PageService>();                 // ★画面追加すると、ここも更新されるみたい
            services.AddSingleton<INavigationService, NavigationService>();     // 画面遷移を行う

            // Core Services
            services.AddSingleton<ISampleDataService, SampleDataService>();     // サンプルデータを提供する
            services.AddSingleton<IFileService, FileService>();                 // ファイルの読み書きを行う。MSIX非使用の場合のみ使用する。
            services.AddSingleton<ISemanticService, SemanticService>();         // SemanticKernel
            services.AddSingleton<ILiteDbService, LiteDbService>();         // データベースの操作を行う

            // ★画面追加すると、ここも更新されるみたい
            // Views and ViewModels
            services.AddTransient<ChatViewModel>();
            services.AddTransient<ChatPage>();
            services.AddTransient<ToDoListViewModel>();
            services.AddTransient<ToDoListPage>();
            services.AddTransient<CharacterSettingsDetailViewModel>();      // キャラクター設定の詳細画面のViewModel
            services.AddTransient<CharacterSettingsDetailPage>();
            services.AddTransient<CharacterSettingsViewModel>();
            services.AddTransient<CharacterSettingsPage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<PersonalDetailViewModel>();
            services.AddTransient<PersonalDetailPage>();
            services.AddTransient<PersonalViewModel>();
            services.AddTransient<PersonalPage>();
            services.AddTransient<WelcomeViewModel>();
            services.AddTransient<WelcomePage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));   // 開発者が設定する固定の設定内容
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: 例外を記録し、適切に処理する。
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    /// <summary>
    /// 起動時処理
    /// 登録されているIActivationServiceを実行する
    /// </summary>
    /// <param name="args"></param>
    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
