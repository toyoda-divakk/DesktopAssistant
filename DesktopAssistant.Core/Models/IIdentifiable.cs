namespace DesktopAssistant.Core.Models;

/// <summary>
/// IDがついていることを保証する
/// </summary>
public interface IIdentifiable
{
    /// <summary>
    /// ID
    /// </summary>
    long Id
    {
        get;
    }
}