using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Enums;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.Contracts.Services;

// TODO:選択中のアシスタントはDBに保存することにしたので、最悪この保存はいらなくなるかもしれない
/// <summary>
/// アシスタント設定を提供
/// </summary>
public interface ICharacterSettingService
{
    /// <summary>
    /// 現在選択中のアシスタント
    /// </summary>
    Character CurrentCharacter
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
    Task SetAndSaveAsync(ICharacterSetting setting);

    /// <summary>
    /// 設定内容を直ちにアプリに反映する
    /// （特に何もしていない）
    /// </summary>
    /// <returns></returns>
    Task SetRequestedSettingAsync();
}
