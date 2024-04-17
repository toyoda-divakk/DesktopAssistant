namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// ローカル設定を読み書きするサービスのインターフェース
/// </summary>
public interface ILocalSettingsService
{
    Task<T?> ReadSettingAsync<T>(string key);

    Task SaveSettingAsync<T>(string key, T value);

    /// <summary>
    /// 非MSIXの場合のみ使用するアプリケーションデータフォルダを取得します。
    /// </summary>
    /// <returns></returns>
    string GetApplicationDataFolder();
}
