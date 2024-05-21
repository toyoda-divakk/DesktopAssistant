using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Enums;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Helpers;

namespace DesktopAssistant.Services;

// TODO:多分使わないので、アシスタント実装できたら消すこと

/// <summary>
/// チャットで使用するアシスタントの設定を管理するサービスを表します。
/// </summary>
public class AssistantSettingService(ILocalSettingsService localSettingsService, ILiteDbService liteDbService) : IAssistantSettingService, IAssistantSetting
{
    // ※保存するのはIDだけでよい、これは持っておくだけ
    private Assistant _currentAssistant = null!;
    public Assistant CurrentAssistant
    {
        get
        {
            if (_currentAssistant == null || _currentAssistant.Id != CurrentAssistantId)
            {
                _currentAssistant = liteDbService.GetTable<Assistant>().First(x => x.Id == CurrentAssistantId);
            }
            return _currentAssistant;
        }
    }

    public long CurrentAssistantId
    {
        get; set;
    }

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
    public async Task SetAndSaveAsync(IAssistantSetting setting)
    {
        // リフレクションで移す
        FieldCopier.CopyProperties<IAssistantSetting>(setting, this);

        _currentAssistant = liteDbService.GetTable<Assistant>().First(x => x.Id == CurrentAssistantId);
        await SetRequestedSettingAsync();      // すぐにアプリに反映
        SaveSetting();
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
        var currentAssistantId = localSettingsService.ReadSetting<long>(nameof(CurrentAssistantId));
        CurrentAssistantId = currentAssistantId == 0 ? 1L : currentAssistantId;
    }

    private void SaveSetting()
    {
        localSettingsService.SaveSetting(nameof(CurrentAssistantId), CurrentAssistant.Id);
    }
}
