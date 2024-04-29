using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAssistant.Core.Enums;

/// <summary>
/// Enterキーに関するキーバインドの種類
/// </summary>
public enum EnterKeyBond
{
    Enter,
    ShiftEnter, // ひょっとしてこれ左のShiftと右のShift区別あったりする？
    CtrlEnter,
    AltEnter
}
