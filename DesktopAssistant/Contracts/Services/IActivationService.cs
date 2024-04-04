namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// 起動時の処理を行うサービスのインターフェース
/// </summary>
public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
