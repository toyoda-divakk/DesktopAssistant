namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// ローカル設定を読み書きするサービスのインターフェース
/// </summary>
public interface ILocalSettingsService
{
    Task<T?> ReadSettingAsync<T>(string key);

    Task SaveSettingAsync<T>(string key, T value);
}
