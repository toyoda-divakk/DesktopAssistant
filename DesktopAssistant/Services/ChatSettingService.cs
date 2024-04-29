using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Enums;
using DesktopAssistant.Helpers;

namespace DesktopAssistant.Services;

/// <summary>
/// チャット画面の設定を管理するサービスを表します。
/// </summary>
public class ChatSettingService(ILocalSettingsService localSettingsService) : IChatSettingService, IChatSetting
{
    public ChatPosition UserPosition { get; set; } = ChatPosition.Right;

    public string UserBackgroundColor { get; set; } = string.Empty;

    public string UserTextColor { get; set; } = string.Empty;

    public ChatPosition AIPosition { get; set; } = ChatPosition.Left;

    public EnterKeyBond KeyBindNewLine { get; set; } = EnterKeyBond.Enter;

    public EnterKeyBond KeyBindSend { get; set; } = EnterKeyBond.CtrlEnter;

    /// <summary>
    /// 初期化処理
    /// ActivationServiceに登録すること
    /// 設定の再読み込み処理の場合も使用する
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        ReLoadSettings();
        await Task.CompletedTask;
    }

    /// <summary>
    /// 生成AIの設定を変更する
    /// </summary>
    /// <param name="setting"></param>
    /// <returns></returns>
    public async Task SetAndSaveAsync(IChatSetting setting)
    {
        // リフレクションで移す
        FieldCopier.CopyProperties<IChatSetting>(setting, this);

        await SetRequestedSettingAsync();      // すぐにアプリに反映
        SaveSetting();  // 切り替えたらすぐにファイル保存
    }

    /// <summary>
    /// 設定内容をアプリに反映する
    /// 特に何も実装しない
    /// </summary>
    /// <returns></returns>
    public async Task SetRequestedSettingAsync()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// 設定の再読み込みを行う
    /// 初めて読み込む場合も使用する
    /// </summary>
    /// <returns></returns>
    private void ReLoadSettings()
    {
        UserPosition = Enum.TryParse(localSettingsService.ReadSetting<string>(nameof(UserPosition)), out ChatPosition userPosition) ? userPosition : ChatPosition.Right;
        UserBackgroundColor = localSettingsService.ReadSetting<string>(nameof(UserBackgroundColor)) ?? "#a3d1ff";
        UserTextColor = localSettingsService.ReadSetting<string>(nameof(UserTextColor)) ?? "#000000";
        AIPosition = Enum.TryParse(localSettingsService.ReadSetting<string>(nameof(AIPosition)), out ChatPosition aiPosition) ? aiPosition : ChatPosition.Left;
        KeyBindNewLine = Enum.TryParse(localSettingsService.ReadSetting<string>(nameof(KeyBindNewLine)), out EnterKeyBond keyBindNewLineStringValue) ? keyBindNewLineStringValue : EnterKeyBond.Enter;
        KeyBindSend = Enum.TryParse(localSettingsService.ReadSetting<string>(nameof(KeyBindSend)), out EnterKeyBond keyBindSendStringValue) ?  keyBindSendStringValue : EnterKeyBond.CtrlEnter;
    }

    private void SaveSetting()
    {
        localSettingsService.SaveSetting(nameof(UserPosition), UserPosition.ToString());
        localSettingsService.SaveSetting(nameof(UserBackgroundColor), UserBackgroundColor);
        localSettingsService.SaveSetting(nameof(UserTextColor), UserTextColor);
        localSettingsService.SaveSetting(nameof(AIPosition), AIPosition.ToString());
        localSettingsService.SaveSetting(nameof(KeyBindNewLine), KeyBindNewLine.ToString());
        localSettingsService.SaveSetting(nameof(KeyBindSend), KeyBindSend.ToString());
    }
}
