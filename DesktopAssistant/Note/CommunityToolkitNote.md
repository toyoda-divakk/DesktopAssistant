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
```
[ObservableProperty]
private string? name;
```

変更時の処理を追加する
```
[ObservableProperty]
private string? name;

partial void OnNameChanging(string? value)
{
    Console.WriteLine($"Name is about to change to {value}");
}

partial void OnNameChanged(string? value)
{
    Console.WriteLine($"Name has changed to {value}");
}
```

変更のたびに通知を発生させたいプロパティ
```
[ObservableProperty]
[NotifyPropertyChangedFor(nameof(FullName))]
private string? name;
```

プロパティが変更されるたびに、コマンドの実行状態を無効にして計算し直す必要がある場合。
```
[ObservableProperty]
[NotifyCanExecuteChangedFor(nameof(MyCommand))]
private string? name;
```

プロパティのバリデーションをトリガーする
```
[ObservableProperty]
[NotifyDataErrorInfo]
[Required]
[MinLength(2)] // Any other validation attributes too...
private string? name;
```

プロパティの変更に対してプロパティ変更メッセージを送信するコードも挿入する  
※Broadcast(oldValue, value);が生成される
```
[ObservableProperty]
[NotifyPropertyChangedRecipients]
private string? name;
```

## コマンド
GreetUserCommandが生成されるので、これをバインドすること。
```
[RelayCommand]
private void GreetUser(User user)
{
    Console.WriteLine($"Hello {user.Name}!");
}
```

非同期コマンド
CancellationTokenをつけることができる。つけなくてもよい。
この時もGreetUserCommandが生成される。
```
[RelayCommand]
private async Task GreetUserAsync(CancellationToken token)
{
    User user = await userService.GetCurrentUserAsync();

    Console.WriteLine($"Hello {user.Name}!");
}
```

実行可否を制御する
AllowConcurrentExecutionsで同時実行を許可する。false:デフォルト。コマンド実行は無効になる。true:コマンド実行がキューイングされる？（要検証）
```
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
```
[RelayCommand(FlowExceptionsToTaskScheduler = true)]
```

非同期操作のキャンセル・コマンド
"（メソッド名）CancelCommand"というコマンドが生成されるので、これを他のUIにバインドすると保留中の非同期処理をキャンセルできる。
```
[RelayCommand(IncludeCancelCommand = true)]
```

