namespace DesktopAssistant.Behaviors;

public enum NavigationViewHeaderMode
{
    /// <summary>
    /// ヘッダーを常に表示する
    /// </summary>
    Always,
    /// <summary>
    /// ヘッダーを表示しない
    /// </summary>
    Never,
    /// <summary>
    /// ページから取得したヘッダーコンテキストがあればそれを表示し、なければデフォルトのヘッダーを表示
    /// </summary>
    Minimal
}
