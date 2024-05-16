using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Enums;
using DesktopAssistant.Core.Models;
using DesktopAssistant.Helpers;

namespace DesktopAssistant.Services;

// TODO:多分使わないので、キャラ実装できたら消すこと

/// <summary>
/// チャットで使用するキャラクターの設定を管理するサービスを表します。
/// </summary>
public class CharacterSettingService(ILocalSettingsService localSettingsService, ILiteDbService liteDbService) : ICharacterSettingService, ICharacterSetting
{
    // ※保存するのはIDだけでよい、これは持っておくだけ
    private Character _currentCharacter = null!;
    public Character CurrentCharacter
    {
        get
        {
            if (_currentCharacter == null || _currentCharacter.Id != CurrentCharacterId)
            {
                _currentCharacter = liteDbService.GetTable<Character>().First(x => x.Id == CurrentCharacterId);
            }
            return _currentCharacter;
        }
    }

    public long CurrentCharacterId
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
    public async Task SetAndSaveAsync(ICharacterSetting setting)
    {
        // リフレクションで移す
        FieldCopier.CopyProperties<ICharacterSetting>(setting, this);

        _currentCharacter = liteDbService.GetTable<Character>().First(x => x.Id == CurrentCharacterId);
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
        var currentCharacterId = localSettingsService.ReadSetting<long>(nameof(CurrentCharacterId));
        CurrentCharacterId = currentCharacterId == 0 ? 1L : currentCharacterId;
    }

    private void SaveSetting()
    {
        localSettingsService.SaveSetting(nameof(CurrentCharacterId), CurrentCharacter.Id);
    }
}
