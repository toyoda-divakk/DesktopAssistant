namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// ローカル設定を読み書きするサービスのインターフェース
/// </summary>
public interface ILocalSettingsService
{
    Task<T?> ReadSettingAsync<T>(string key);

    Task SaveSettingAsync<T>(string key, T value);

    /// <summary>
    /// アプリケーションデータフォルダのパスを取得します。
    /// </summary>
    /// <returns></returns>
    string GetApplicationDataFolder();
}
