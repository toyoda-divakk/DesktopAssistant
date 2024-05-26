using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopAssistant.Core.Contracts.Interfaces;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DesktopAssistant.Core.Contracts.Services;

/// <summary>
/// とりあえずSemanticKernelをラッピングする
/// </summary>
public interface ISemanticService
{
    /// <summary>
    /// 接続テスト
    /// </summary>
    /// <param name="settings">API設定</param>
    /// <param name="hello">送信文</param>
    /// <returns>内容</returns>
    Task<string> TestGenerativeAIAsync(IApiSetting settings, string hello);

    ///// <summary>
    ///// Kernelを引数の設定で初期化する。
    ///// </summary>
    ///// <param name="settings"></param>
    //public void Initialize(IApiSetting settings);

    /// <summary>
    /// 設定とプロンプトを指定してチャットを生成する
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="prompt"></param>
    /// <returns></returns>
    public ChatHistory InitializeChat(IApiSetting settings, string prompt);

    /// <summary>
    /// チャットを生成する
    /// 履歴に追加する
    /// </summary>
    /// <param name="history">今までの会話</param>
    /// <param name="userMessage">ユーザの発言</param>
    /// <returns>返答</returns>
    public Task<string> GenerateChatAsync(ChatHistory history, string userMessage);

}
