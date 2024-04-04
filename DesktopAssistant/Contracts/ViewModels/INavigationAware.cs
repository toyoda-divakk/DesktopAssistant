namespace DesktopAssistant.Contracts.ViewModels;

/// <summary>
/// 画面間の遷移時に発生するイベントに対応するメソッドを提供
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// 画面に遷移した直後に呼び出される
    /// 遷移先の画面が初期化される際に必要な処理を実行する
    /// </summary>
    /// <param name="parameter">遷移元の画面から渡されたパラメータ</param>
    void OnNavigatedTo(object parameter);

    /// <summary>
    /// 画面から遷移する直前に呼び出される
    /// 遷移元の画面が破棄される前に必要な後処理を実行する（データを保存するなど）
    /// </summary>
    void OnNavigatedFrom();
}
