namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// ページの型を取得するためのサービス
/// </summary>
public interface IPageService
{
    Type GetPageType(string key);
}
