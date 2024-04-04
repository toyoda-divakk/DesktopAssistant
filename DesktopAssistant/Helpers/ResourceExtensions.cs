using Microsoft.Windows.ApplicationModel.Resources;

namespace DesktopAssistant.Helpers;

public static class ResourceExtensions
{
    private static readonly ResourceLoader _resourceLoader = new();

    /// <summary>
    /// Resourceから指定したキーの文字列を取得する
    /// </summary>
    /// <param name="resourceKey"></param>
    /// <returns></returns>
    public static string GetLocalized(this string resourceKey) => _resourceLoader.GetString(resourceKey);
}
