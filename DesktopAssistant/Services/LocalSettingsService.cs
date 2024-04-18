using System.Text.Json;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Helpers;
using DesktopAssistant.Models;

using Microsoft.Extensions.Options;

using Windows.ApplicationModel;
using Windows.Storage;

namespace DesktopAssistant.Services;

/// <summary>
/// ユーザーが設定したローカル設定を読み書きするサービスを表します。
/// MSIXの使用有無での違いはなるべくこのサービスで吸収する。
/// </summary>
public class LocalSettingsService : ILocalSettingsService
{
    private const string _defaultApplicationDataFolder = "DesktopAssistant/ApplicationData";
    private const string _defaultLocalSettingsFile = "LocalSettings.json";

    private readonly IFileService _fileService; // MSIX非使用の場合のみ使用
    private readonly LocalSettingsOptions _options;

    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _applicationDataFolder;
    private readonly string _localsettingsFile; // 実際に保存しているjsonファイル

    public string ApplicationDataFolder => _applicationDataFolder;

    /// <summary>
    /// MSIX非使用の場合の設定
    /// MSIXはApplicationData.Current.LocalSettings.Valuesを使用する
    /// </summary>
    private readonly IDictionary<string, object> _settings;

    public LocalSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options)
    {
        _fileService = fileService;
        _options = options.Value;

        _applicationDataFolder = RuntimeHelper.IsMSIX
            ? Path.Combine(ApplicationData.Current.LocalFolder.Path, _options.ApplicationDataFolder ?? _defaultApplicationDataFolder) // C:\\Users\\UserName\\AppData\\Local\\Packages\\83a7990a-84aa-479c-9662-45da248ee082_gcfy10y4r1fd6\\LocalState\\DesktopAssistant/ApplicationData
            : Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? _defaultApplicationDataFolder);
        _localsettingsFile = _options.LocalSettingsFileName ?? _defaultLocalSettingsFile;
        _settings = RuntimeHelper.IsMSIX
            ? new Dictionary<string, object>()
            : _fileService.Read<IDictionary<string, object>>(_applicationDataFolder, _localsettingsFile) ?? new Dictionary<string, object>();
        // データフォルダが無かったら作成
        if (!Directory.Exists(_applicationDataFolder))
        {
            Directory.CreateDirectory(_applicationDataFolder);
        }
    }


    /// <summary>
    /// 設定値を取得する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T? ReadSetting<T>(string key)
    {
        if (RuntimeHelper.IsMSIX)
        {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj))
            {
                return JsonSerializer.Deserialize<T>((string)obj);
            }
        }
        else
        {
            // 使わない
            if (_settings != null && _settings.TryGetValue(key, out var obj))
            {
                return JsonSerializer.Deserialize<T>((string)obj);
            }
        }

        return default;
    }

    // 画面で設定変更時に呼び出される。ラジオボタンなどを切り替えたらすぐにファイルに保存している。
    /// <summary>
    /// 値を入れてすぐにファイルに保存する
    /// valueはどんなクラスもjsonに変換して格納する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public void SaveSetting<T>(string key, T value)
    {
        if (RuntimeHelper.IsMSIX)
        {
            ApplicationData.Current.LocalSettings.Values[key] = JsonSerializer.Serialize(value);
        }
        else
        {
            // 使わない
            _settings[key] = JsonSerializer.Serialize(value);
            _fileService.Save(_applicationDataFolder, _localsettingsFile, _settings);
        }
    }
}
