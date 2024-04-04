using DesktopAssistant.Contracts.Services;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Xaml.Interactivity;

namespace DesktopAssistant.Behaviors;

/// <summary>
/// NavigationViewコントロールのヘッダーを制御する
/// ナビゲーションメニューのヘッダーを動的に更新するためのロジックを提供する
/// </summary>
public class NavigationViewHeaderBehavior : Behavior<NavigationView>
{
    private static NavigationViewHeaderBehavior? _current;

    private Page? _currentPage;

    public DataTemplate? DefaultHeaderTemplate
    {
        get; set;
    }

    public object DefaultHeader
    {
        get => GetValue(DefaultHeaderProperty);
        set => SetValue(DefaultHeaderProperty, value);
    }

    public static readonly DependencyProperty DefaultHeaderProperty =
        DependencyProperty.Register("DefaultHeader", typeof(object), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => _current!.UpdateHeader()));

    public static NavigationViewHeaderMode GetHeaderMode(Page item) => (NavigationViewHeaderMode)item.GetValue(HeaderModeProperty);

    public static void SetHeaderMode(Page item, NavigationViewHeaderMode value) => item.SetValue(HeaderModeProperty, value);

    public static readonly DependencyProperty HeaderModeProperty =
        DependencyProperty.RegisterAttached("HeaderMode", typeof(bool), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(NavigationViewHeaderMode.Always, (d, e) => _current!.UpdateHeader()));

    public static object GetHeaderContext(Page item) => item.GetValue(HeaderContextProperty);

    public static void SetHeaderContext(Page item, object value) => item.SetValue(HeaderContextProperty, value);

    public static readonly DependencyProperty HeaderContextProperty =
        DependencyProperty.RegisterAttached("HeaderContext", typeof(object), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => _current!.UpdateHeader()));

    public static DataTemplate GetHeaderTemplate(Page item) => (DataTemplate)item.GetValue(HeaderTemplateProperty);

    public static void SetHeaderTemplate(Page item, DataTemplate value) => item.SetValue(HeaderTemplateProperty, value);

    public static readonly DependencyProperty HeaderTemplateProperty =
        DependencyProperty.RegisterAttached("HeaderTemplate", typeof(DataTemplate), typeof(NavigationViewHeaderBehavior), new PropertyMetadata(null, (d, e) => _current!.UpdateHeaderTemplate()));

    /// <summary>
    /// ナビゲーションサービスのNavigatedイベントにイベントハンドラを追加し
    /// ページのナビゲーションが行われたときにヘッダーを更新するための処理
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();

        var navigationService = App.GetService<INavigationService>();
        navigationService.Navigated += OnNavigated;

        _current = this;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        var navigationService = App.GetService<INavigationService>();
        navigationService.Navigated -= OnNavigated;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame && frame.Content is Page page)
        {
            _currentPage = page;

            UpdateHeader();
            UpdateHeaderTemplate();
        }
    }

    /// <summary>
    /// 現在表示されているページのヘッダーモードを取得し、それに基づいてヘッダーの表示を制御します。
    /// </summary>
    private void UpdateHeader()
    {
        if (_currentPage != null)
        {
            var headerMode = GetHeaderMode(_currentPage);
            if (headerMode == NavigationViewHeaderMode.Never)
            {
                // ヘッダーを非表示にし、AlwaysShowHeaderプロパティをfalseに設定
                AssociatedObject.Header = null;
                AssociatedObject.AlwaysShowHeader = false;
            }
            else
            {
                // Never, Always以外の場合、ページから取得したヘッダーコンテキストがあればそれを表示し、なければデフォルトのヘッダーを表示します。
                var headerFromPage = GetHeaderContext(_currentPage);
                if (headerFromPage != null)
                {
                    AssociatedObject.Header = headerFromPage;
                }
                else
                {
                    AssociatedObject.Header = DefaultHeader;
                }

                if (headerMode == NavigationViewHeaderMode.Always)
                {
                    // ヘッダーを常に表示するためにAlwaysShowHeaderプロパティをtrueに設定
                    AssociatedObject.AlwaysShowHeader = true;
                }
                else
                {
                    AssociatedObject.AlwaysShowHeader = false;
                }
            }
        }
    }

    /// <summary>
    /// 現在表示されているページからヘッダーテンプレートを取得し、それをNavigationViewコントロールのHeaderTemplateプロパティに設定します。
    /// テンプレートがない場合は、デフォルトのヘッダーテンプレートを使用します。
    /// </summary>
    private void UpdateHeaderTemplate()
    {
        if (_currentPage != null)
        {
            var headerTemplate = GetHeaderTemplate(_currentPage);
            AssociatedObject.HeaderTemplate = headerTemplate ?? DefaultHeaderTemplate;
        }
    }
}
