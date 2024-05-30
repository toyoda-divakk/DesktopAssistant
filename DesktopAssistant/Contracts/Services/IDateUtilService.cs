namespace DesktopAssistant.Contracts.Services;

/// <summary>
/// 時間に関する処理を行うサービス
/// </summary>
public interface IDateUtilService
{
    /// <summary>
    /// 引数のDateTimeと現在時刻を比較して、その時刻まであと何日かを表示する。1日未満ならあと何時間か表示する。現在時刻が過ぎていたら"期限切れ"を返す。
    /// </summary>
    /// <param name="dateTime">判定する時刻</param>
    /// <returns></returns>
    string GetRemainingTime(DateTime dateTime);
}
