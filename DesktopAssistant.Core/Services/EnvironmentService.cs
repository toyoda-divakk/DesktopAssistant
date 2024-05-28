using System.Text;
using System.Text.Json;
using DesktopAssistant.Core.Contracts.Services;

namespace DesktopAssistant.Core.Services;

/// <summary>
/// 環境変数を読むだけかな
/// </summary>
public class EnvironmentService : IEnvironmentService
{
    /// <summary>
    /// 環境変数を読む
    /// </summary>
    /// <param name="variableName">変数名</param>
    /// <returns></returns>
    public string GetEnvironmentVariable(string variableName)
    {
        return Environment.GetEnvironmentVariable(variableName);
    }

}
