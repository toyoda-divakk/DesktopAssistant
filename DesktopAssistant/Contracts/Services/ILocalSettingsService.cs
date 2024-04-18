namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// ローカル設定を読み書きするサービスのインターフェース
/// </summary>
public interface ILocalSettingsService
{
    T? ReadSetting<T>(string key);

    void SaveSetting<T>(string key, T value);

    /// <summary>
    /// アプリケーションのデータフォルダ
    /// </summary>
    string ApplicationDataFolder
    {
        get;
    }
}
