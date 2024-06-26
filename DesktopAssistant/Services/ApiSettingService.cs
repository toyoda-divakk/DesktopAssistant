﻿using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Enums;
using DesktopAssistant.Helpers;

namespace DesktopAssistant.Services;

/// <summary>
/// API設定を管理するサービスを表します。
/// </summary>
public class ApiSettingService(ILocalSettingsService localSettingsService, IEnvironmentService environmentService) : IApiSettingService, IApiSetting
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
    /// APIテストが済んでいるか
    /// </summary>
    public bool IsApiTested { get; set; } = true;           // やっぱ面倒なのでtrueにしておく。後で実装するならfalseにする

    public IApiSetting GetSettings() => this;

    /// <summary>
    /// 初期化処理
    /// ActivationServiceに登録すること
    /// 設定の再読み込み処理の場合も使用する
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        ReLoadSettings();
        await Task.CompletedTask;
    }

    /// <summary>
    /// 生成AIの設定を変更する
    /// </summary>
    /// <param name="setting"></param>
    /// <returns></returns>
    public async Task SetAndSaveAsync(IApiSetting setting)
    {
        // リフレクションで移す
        FieldCopier.CopyProperties<IApiSetting>(setting, this);

        await SetRequestedSettingAsync();      // すぐにアプリに反映
        SaveSetting();  // 切り替えたらすぐにファイル保存
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
    private void ReLoadSettings()
    {

        var GenerativeAIString = localSettingsService.ReadSetting<string>(nameof(GenerativeAI));
        if (Enum.TryParse(GenerativeAIString, out GenerativeAI cacheValue))
        {
            GenerativeAI = cacheValue;
        }
        else
        {
            GenerativeAI = GenerativeAI.OpenAI;
        }

        // 開発効率化のため、環境変数に設定があれば読む
        var tempKey = environmentService.GetEnvironmentVariable("AzureOpenAIKey") ?? string.Empty;
        var tempModel = environmentService.GetEnvironmentVariable("AzureOpenAIModel") ?? string.Empty;
        var tempEndpoint = environmentService.GetEnvironmentVariable("AzureOpenAIEndpoint") ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(tempKey))
        {
            GenerativeAI = GenerativeAI.AzureOpenAI;
        }

        OpenAIKey = localSettingsService.ReadSetting<string>(nameof(OpenAIKey)) ?? string.Empty;
        OpenAIModel = localSettingsService.ReadSetting<string>(nameof(OpenAIModel)) ?? string.Empty;
        AzureOpenAIKey = localSettingsService.ReadSetting<string>(nameof(AzureOpenAIKey)) ?? tempKey;
        AzureOpenAIModel = localSettingsService.ReadSetting<string>(nameof(AzureOpenAIModel)) ?? tempModel;
        AzureOpenAIEndpoint = localSettingsService.ReadSetting<string>(nameof(AzureOpenAIEndpoint)) ?? tempEndpoint;
        IsApiTested = localSettingsService.ReadSetting<bool>(nameof(IsApiTested));
    }

    private void SaveSetting()
    {
        localSettingsService.SaveSetting(nameof(GenerativeAI), GenerativeAI.ToString());
        localSettingsService.SaveSetting(nameof(OpenAIKey), OpenAIKey);
        localSettingsService.SaveSetting(nameof(OpenAIModel), OpenAIModel);
        localSettingsService.SaveSetting(nameof(AzureOpenAIKey), AzureOpenAIKey);
        localSettingsService.SaveSetting(nameof(AzureOpenAIModel), AzureOpenAIModel);
        localSettingsService.SaveSetting(nameof(AzureOpenAIEndpoint), AzureOpenAIEndpoint);
        localSettingsService.SaveSetting(nameof(IsApiTested), IsApiTested);
    }
}
