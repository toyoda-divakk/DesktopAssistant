using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Enums;

namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// API設定を提供
/// </summary>
public interface IApiSettingService
{
    // 保存内容
    /// <summary>
    /// 生成AIサービス
    /// </summary>
    GenerativeAI GenerativeAI
    {
        get;
    }
    /// <summary>
    /// OpenAIのAPIキー
    /// </summary>
    string OpenAIKey
    {
        get;
    }
    /// <summary>
    /// OpenAIのモデル名
    /// </summary>
    string OpenAIModel
    {
        get;
    }
    /// <summary>
    /// AzureOpenAIのAPIキー
    /// </summary>
    string AzureOpenAIKey
    {
        get;
    }
    /// <summary>
    /// AzureOpenAIのデプロイメント名
    /// </summary>
    string AzureOpenAIModel
    {
        get;
    }
    /// <summary>
    /// AzureOpenAIのエンドポイント
    /// </summary>
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
    /// 変更内容を保存し、アプリに反映する
    /// </summary>
    /// <param name="generativeAI"></param>
    /// <returns></returns>
    Task SetAndSaveAsync(IApiSetting setting);

    /// <summary>
    /// 設定内容を直ちにアプリに反映する
    /// （特に何もしていない）
    /// </summary>
    /// <returns></returns>
    Task SetRequestedSettingAsync();

    /// <summary>
    /// このサービスの設定をIApiSetting形式で取得する
    /// </summary>
    /// <returns></returns>
    IApiSetting GetSettings();
}
