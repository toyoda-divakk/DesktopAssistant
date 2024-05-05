using CommunityToolkit.Mvvm.ComponentModel;

using DesktopAssistant.Contracts.Services;
using DesktopAssistant.ViewModels;
using DesktopAssistant.ViewModels.Popup;
using DesktopAssistant.Views;
using DesktopAssistant.Views.Popup;

using Microsoft.UI.Xaml.Controls;

namespace DesktopAssistant.Services;

/// <summary>
/// ページの型を取得するサービス
/// </summary>
public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = [];

    public PageService()
    {
        Configure<MainViewModel, MainPage>();
        Configure<PersonalViewModel, PersonalPage>();
        Configure<PersonalDetailViewModel, PersonalDetailPage>();
        Configure<SettingsViewModel, SettingsPage>();
        Configure<ToDoListViewModel, ToDoListPage>();
        Configure<ChatViewModel, ChatPage>();
        Configure<CharacterImageViewModel, CharacterImagePage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
