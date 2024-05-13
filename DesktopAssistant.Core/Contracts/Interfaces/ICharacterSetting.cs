using DesktopAssistant.Core.Enums;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.Core.Contracts.Interfaces;

/// <summary>
/// キャラクターに関する設定項目のインターフェース
/// </summary>
public interface ICharacterSetting
{
    // クラスだとリフレクションで映すのが面倒なのでIDのみ
    public long CurrentCharacterId
    {
        get; set;
    }
}

