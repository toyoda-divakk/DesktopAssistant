namespace DesktopAssistant.Models;

// ※開発者しか触れん方な。ユーザ側は"LocalSettings.json"。
/// <summary>
/// appsettings.jsonの内容
/// </summary>
public class LocalSettingsOptions
{
    public string? ApplicationDataFolder
    {
        get; set;
    }

    public string? LocalSettingsFile
    {
        get; set;
    }
}
