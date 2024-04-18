using DesktopAssistant.Core.Models;

namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// とりあえずLiteDBを使ってみる
/// アプリ固有の操作はなるべく実装しないように
/// </summary>
public interface ILiteDbService
{
    //List<TodoTask> Test();

    /// <summary>
    /// データファイルが存在しているか
    /// </summary>
    /// <returns></returns>
    bool IsExistDatabase();

    /// <summary>
    /// データファイルが無ければ作成
    /// あれば削除して再作成
    /// </summary>
    void CreateOrInitializeDatabase();

    /// <summary>
    /// 任意のテーブルを全件取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    List<T> GetTable<T>();

    /// <summary>
    /// 任意のテーブルにデータを追加
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    void Insert<T>(T data);
}
