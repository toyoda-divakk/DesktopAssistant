using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Enums;

namespace DesktopAssistant.Services;

/// <summary>
/// テーマの選択を管理するサービスを表します。
/// </summary>
public class ApiSettingService(ILocalSettingsService localSettingsService) : IApiSettingService
{
    private const string SettingsKey = "AppGenerativeAI";

    public GenerativeAI GenerativeAI { get; set; } = GenerativeAI.OpenAI;

    /// <summary>
    /// 初期化処理
    /// ActivationServiceに登録すること
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        GenerativeAI = await LoadSettingsAsync();
        await Task.CompletedTask;
    }

    /// <summary>
    /// 生成AIを変更する
    /// </summary>
    /// <param name="generativeAI"></param>
    /// <returns></returns>
    public async Task SetGenerativeAIAsync(GenerativeAI generativeAI)
    {
        GenerativeAI = generativeAI;

        await SetRequestedSettingAsync();      // すぐにアプリに反映
        await SaveSettingAsync(GenerativeAI);  // 切り替えたらすぐにファイル保存
    }

    /// <summary>
    /// 設定内容をアプリに反映する
    /// </summary>
    /// <returns></returns>
    public async Task SetRequestedSettingAsync()
    {
        // TODO:表示する設定項目の変更とかかな…？
        await Task.CompletedTask;
    }

    private async Task<GenerativeAI> LoadSettingsAsync()
    {
        var valueName = await localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (Enum.TryParse(valueName, out GenerativeAI cacheValue))
        {
            return cacheValue;
        }

        return GenerativeAI.OpenAI;
    }

    private async Task SaveSettingAsync(GenerativeAI val)
    {
        await localSettingsService.SaveSettingAsync(SettingsKey, val.ToString());
    }
}
