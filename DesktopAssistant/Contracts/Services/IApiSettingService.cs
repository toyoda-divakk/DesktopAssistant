using DesktopAssistant.Core.Enums;

namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// API設定を提供
/// </summary>
public interface IApiSettingService
{
    // 保存内容
    GenerativeAI GenerativeAI
    {
        get;
    }
    string OpenAIKey
    {
        get;
    }
    string OpenAIModel
    {
        get;
    }
    string AzureOpenAIKey
    {
        get;
    }
    string AzureOpenAIModel
    {
        get;
    }
    string AzureOpenAIEndpoint
    {
        get;
    }

    /// <summary>
    /// 初期化処理
    /// ActivationServiceに登録すること
    /// </summary>
    /// <returns></returns>
    Task InitializeAsync();

    /// <summary>
    /// 生成AIを変更する
    /// </summary>
    /// <param name="generativeAI"></param>
    /// <returns></returns>
    Task SetGenerativeAIAsync(IApiSetting setting);

    /// <summary>
    /// 設定内容をアプリに反映する
    /// </summary>
    /// <returns></returns>
    Task SetRequestedSettingAsync();
}
