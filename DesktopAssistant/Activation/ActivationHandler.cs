namespace DesktopAssistant.Activation;

/// <summary>
/// 起動時処理を行うためのインターフェース
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ActivationHandler<T> : IActivationHandler
    where T : class
{
    // このメソッドをオーバーライドして、アクティベーションを処理するかどうかのロジックを追加します。
    protected virtual bool CanHandleInternal(T args) => true;

    // このメソッドをオーバーライドして、起動ハンドラのロジックを追加します。
    protected abstract Task HandleInternalAsync(T args);

    public bool CanHandle(object args) => args is T && CanHandleInternal((args as T)!);

    public async Task HandleAsync(object args) => await HandleInternalAsync((args as T)!);
}
