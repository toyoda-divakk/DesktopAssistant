namespace DesktopAssistant.Core.Contracts.Services;

/// <summary>
/// 環境変数を読むだけかな
/// </summary>
public interface IEnvironmentService
{
    string GetEnvironmentVariable(string variableName);
}
