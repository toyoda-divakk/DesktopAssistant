namespace DesktopAssistant.Core.Contracts.Services;

/// <summary>
/// ファイルの読み書きを行うインタフェース
/// </summary>
public interface IFileService
{
    T Read<T>(string folderPath, string fileName);

    void Save<T>(string folderPath, string fileName, T content);

    void Delete(string folderPath, string fileName);
}
