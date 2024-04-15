using DesktopAssistant.Core.Enums;

namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// API設定を提供
/// </summary>
public interface IApiSettingService
{
    GenerativeAI GenerativeAI
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
    Task SetGenerativeAIAsync(GenerativeAI theme);

    /// <summary>
    /// 設定内容をアプリに反映する
    /// </summary>
    /// <returns></returns>
    Task SetRequestedSettingAsync();
}
