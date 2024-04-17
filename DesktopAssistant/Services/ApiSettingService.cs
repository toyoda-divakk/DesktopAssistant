using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Enums;
using DesktopAssistant.Helpers;

namespace DesktopAssistant.Services;

/// <summary>
/// テーマの選択を管理するサービスを表します。
/// </summary>
public class ApiSettingService(ILocalSettingsService localSettingsService) : IApiSettingService, IApiSetting
{
    /// <summary>
    /// 生成AIサービス
    /// </summary>
    public GenerativeAI GenerativeAI { get; set; } = GenerativeAI.OpenAI;

    /// <summary>
    /// OpenAIのAPIキー
    /// </summary>
    public string OpenAIKey { get; set; } = string.Empty;

    /// <summary>
    /// OpenAIのモデル名
    /// </summary>
    public string OpenAIModel { get; set; } = string.Empty;

    /// <summary>
    /// AzureOpenAIのAPIキー
    /// </summary>
    public string AzureOpenAIKey { get; set; } = string.Empty;

    /// <summary>
    /// AzureOpenAIのデプロイメント名
    /// </summary>
    public string AzureOpenAIModel { get; set; } = string.Empty;

    /// <summary>
    /// AzureOpenAIのエンドポイント
    /// </summary>
    public string AzureOpenAIEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// 初期化処理
    /// ActivationServiceに登録すること
    /// 設定の再読み込み処理の場合も使用する
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        await ReLoadSettingsAsync();
        await Task.CompletedTask;
    }

    /// <summary>
    /// 生成AIの設定を変更する
    /// </summary>
    /// <param name="setting"></param>
    /// <returns></returns>
    public async Task SetGenerativeAIAsync(IApiSetting setting)
    {
        // リフレクションで移す
        FieldCopier.CopyProperties<IApiSetting>(setting, this);

        await SetRequestedSettingAsync();      // すぐにアプリに反映
        await SaveSettingAsync();  // 切り替えたらすぐにファイル保存
    }

    /// <summary>
    /// 設定内容をアプリに反映する
    /// 特に何も実装しない
    /// </summary>
    /// <returns></returns>
    public async Task SetRequestedSettingAsync()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// 設定の再読み込みを行う
    /// 初めて読み込む場合も使用する
    /// </summary>
    /// <returns></returns>
    private async Task ReLoadSettingsAsync()
    {
        var GenerativeAIString = await localSettingsService.ReadSettingAsync<string>(nameof(GenerativeAI));
        if (Enum.TryParse(GenerativeAIString, out GenerativeAI cacheValue))
        {
            GenerativeAI = cacheValue;
        }
        else
        {
            GenerativeAI = GenerativeAI.OpenAI;
        }
        OpenAIKey = await localSettingsService.ReadSettingAsync<string>(nameof(OpenAIKey)) ?? string.Empty;
        OpenAIModel = await localSettingsService.ReadSettingAsync<string>(nameof(OpenAIModel)) ?? string.Empty;
        AzureOpenAIKey = await localSettingsService.ReadSettingAsync<string>(nameof(AzureOpenAIKey)) ?? string.Empty;
        AzureOpenAIModel = await localSettingsService.ReadSettingAsync<string>(nameof(AzureOpenAIModel)) ?? string.Empty;
        AzureOpenAIEndpoint = await localSettingsService.ReadSettingAsync<string>(nameof(AzureOpenAIEndpoint)) ?? string.Empty;
    }

    private async Task SaveSettingAsync()
    {
        await localSettingsService.SaveSettingAsync(nameof(GenerativeAI), GenerativeAI.ToString());
        await localSettingsService.SaveSettingAsync(nameof(OpenAIKey), OpenAIKey);
        await localSettingsService.SaveSettingAsync(nameof(OpenAIModel), OpenAIModel);
        await localSettingsService.SaveSettingAsync(nameof(AzureOpenAIKey), AzureOpenAIKey);
        await localSettingsService.SaveSettingAsync(nameof(AzureOpenAIModel), AzureOpenAIModel);
        await localSettingsService.SaveSettingAsync(nameof(AzureOpenAIEndpoint), AzureOpenAIEndpoint);
    }
}
