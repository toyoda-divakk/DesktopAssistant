namespace DesktopAssistant.Models;

// ※開発者しか触れん方な。ユーザ側は"LocalSettings.json"。
/// <summary>
/// appsettings.jsonの内容
/// </summary>
public class LocalSettingsOptions
{
    /// <summary>
    /// 非MSIXの場合のみ使用するフォルダ
    /// </summary>
    public string? ApplicationDataFolder
    {
        get; set;
    }

    /// <summary>
    /// "LocalSettings.json"
    /// </summary>
    public string? LocalSettingsFileName
    {
        get; set;
    }
}
