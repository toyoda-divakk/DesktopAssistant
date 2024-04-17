using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopAssistant.Core.Contracts.Interfaces;

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
}
