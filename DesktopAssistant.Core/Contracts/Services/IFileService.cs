namespace DesktopAssistant.Core.Contracts.Services;

/// <summary>
/// ファイルの読み書きを行うインタフェース
/// MSIX非使用の場合のみ使用すること
/// </summary>
public interface IFileService
{
    T Read<T>(string folderPath, string fileName);

    void Save<T>(string folderPath, string fileName, T content);

    void Delete(string folderPath, string fileName);
}
