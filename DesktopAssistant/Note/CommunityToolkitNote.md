# CommunityToolkitについて
https://qiita.com/hqf00342/items/d12bb669d1ac6fed6ab6
https://shikaku-sh.hatenablog.com/entry/c-sharp-communitytoolkit-mvvm-statup-820

## サンプル
https://github.com/CommunityToolkit/MVVM-Samples

# 基本
[INotifyPropertyChanged]をViewModelクラスに実装することで、プロパティの変更を通知できる。
classはpertialにすること。

## プロパティ
アクセスするときはPascalCaseで指定すること（自動生成されるため）。
```csharp
[ObservableProperty]
private string? name;
```

変更時の処理を追加する
※実行順は以下の通り。そして、引数はこの通りに書かなければいけない。
```csharp
[ObservableProperty]
private string? name;

partial void OnNameChanging(string value)
{
    Console.WriteLine($"Nameプロパティが{value}に変更されようとしてます");
}
partial void OnNameChanging(string? oldValue, string newValue)
{
    Console.WriteLine($"{oldValue}が{newValue}に変更されようとしてます");
}

partial void OnNameChanged(string value)
{
    Console.WriteLine($"Nameプロパティが{value}に変更されました");
}

partial void OnNameChanged(string? oldValue, string newValue)
{
    Console.WriteLine($"{oldValue}が{newValue}に変更されました");
}
```

変更のたびに通知を発生させたいプロパティ
```csharp
[ObservableProperty]
[NotifyPropertyChangedFor(nameof(FullName))]
private string? name;
```

プロパティが変更されるたびに、コマンドの実行状態を無効にして計算し直す必要がある場合。
```csharp
[ObservableProperty]
[NotifyCanExecuteChangedFor(nameof(MyCommand))]
private string? name;
```

プロパティのバリデーションをトリガーする
※xamlやC#の詳しい実装方法はMVVM Toolkit Sample Appを参照。
```csharp
public class RegistrationForm : ObservableValidator
```
```csharp
[ObservableProperty]
[NotifyDataErrorInfo]
[Required]
[MinLength(2)] // Any other validation attributes too...
private string? name;
```



## コマンド
GreetUserCommandが生成されるので、これをバインドすること。
OnItemClickだと、Onが消されてItemClickCommandになる。
```csharp
[RelayCommand]
private void GreetUser(User user)
{
    Console.WriteLine($"Hello {user.Name}!");
}
```

非同期コマンド
CancellationTokenをつけることができる。つけなくてもよい。
この時もGreetUserCommandが生成される。
```csharp
[RelayCommand]
private async Task GreetUserAsync(CancellationToken token)
{
    User user = await userService.GetCurrentUserAsync();

    Console.WriteLine($"Hello {user.Name}!");
}
```

実行可否を制御する
AllowConcurrentExecutionsで同時実行を許可する。false:デフォルト。コマンド実行は無効になる。true:コマンド実行がキューイングされる？（要検証）
```csharp
[RelayCommand(CanExecute = nameof(CanGreetUser), AllowConcurrentExecutions = false)]
private void GreetUser(User? user)
{
    Console.WriteLine($"Hello {user!.Name}!");
}

private bool CanGreetUser(User? user)
{
    return user is not null;
}
```

非同期例外の処理
多分使わないので説明省略
```csharp
[RelayCommand(FlowExceptionsToTaskScheduler = true)]
```

非同期操作のキャンセル・コマンド
"（メソッド名）CancelCommand"というコマンドが生成されるので、これを他のUIにバインドすると保留中の非同期処理をキャンセルできる。
```
[RelayCommand(IncludeCancelCommand = true)]
```

## Animation
SetListDataItemForNextConnectedAnimationとは？
詳しくはCommunityToolkitサンプルアプリの"Connected Animations"を参照。（WinUI3の機能らしい）
https://learn.microsoft.com/ja-jp/windows/apps/design/motion/connected-animation


# Broadcast
なんか2つあるけど？
※Broadcast(oldValue, value, nameof(Name));が生成される
```csharp
[ObservableProperty]
[AlsoBroadcastChange]
private string name;
```

プロパティの変更に対してプロパティ変更メッセージを送信するコードも挿入する  
※Broadcast(oldValue, value);が生成される
```csharp
[ObservableProperty]
[NotifyPropertyChangedRecipients]
private string? name;
```

## メッセンジャー
ブロードキャストはこのメッセンジャーを用いて行われます。
```csharp
    public MainWindowViewModel() : this(WeakReferenceMessenger.Default)
    {
    }

    public MainWindowViewModel(IMessenger messenger)
    {
        Messenger = messenger;
    }
```
独自で実装したコンストラクタが存在する場合、上記のコンストラクタは生成されません。
そのため、コンストラクタを自力実装する場合（≒たいていの場合）、Messengerプロパティにインスタンスをセットしてあげる必要があります。
ObservableRecipientを継承する場合は↑の必要はない。

"Name"プロパティ変更の通知を受け取る方法
```csharp
Messenger.Register<MainWindowViewModel, PropertyChangedMessage<string>>(this, static (recipient, message) =>
{
    //NotifyPropertyChangedを購読するのと同じように書ける
    switch (message.PropertyName)
    {
        case nameof(Name):
            //なんかする
            break;
    }
});
```

## 自分で送受信する場合
必要に応じてメッセージを定義します。
ValueChangedMessage<T>を継承すると簡単。
```csharp
internal class MyMessage : ValueChangedMessage<string>
{
    public MyMessage(string value) : base(value) { }
}
```

WeakReferenceMessengerか、StrongReferenceMessengerを選ぶ。
WeakReferenceMessengerは弱参照を使うので、メッセージを受け取る側がGCされると自動的に登録が解除される。
StrongReferenceMessengerは強参照を使うので、メッセージを受け取る側がGCされても登録が解除されない。メモリリークに注意。

送信
```csharp
    [RelayCommand]
    private void SendMessage()
    {
       　WeakReferenceMessenger.Default.Send<MyMessage>(new("Mesage from MainWindowViewModel"));
    }
```

受信
コンストラクタに書く。
```csharp
    public SubWindowViewModel()
    {
        WeakReferenceMessenger.Default.Register<SubWindowViewModel, MyMessage>(this, static (s, e) =>
        {
            System.Diagnostics.Debug.WriteLine($"Received: {e.Value}");
        });
    }
```

単方向の送受信だけでなく、レスポンスを要求することもできます。
RequestMessage<T>を継承すると簡単。詳しくは↓
https://qiita.com/kk-river/items/d974b02f6c4010433a9e

