using DesktopAssistant.Core.Enums;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.Core.Contracts.Interfaces;

/// <summary>
/// キャラクターに関する設定項目のインターフェース
/// </summary>
public interface ICharacterSetting
{
    ///// <summary>
    ///// 現在選択中のキャラクター
    ///// </summary>
    //Character CurrentCharacter
    //{
    //    get;
    //}

    // クラスだとリフレクションで映すのが面倒なのでIDのみ
    public long CurrentCharacterId
    {
        get; set;
    }
}

