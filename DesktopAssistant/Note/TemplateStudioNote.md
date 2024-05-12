# .NET8.0に上げる
最初は7.0なので、すぐに8.0に上げちゃってOK。  
プロジェクトを右クリックして、プロパティを開いて、ターゲットフレームワークを8.0に変更する。  
そうすると、MSTestがめっちゃエラー出してきてダルいので削除してしまう。

## RuntimeIdentifiersを消してみたが、良かったのだろうか？
```
<RuntimeIdentifiers>win10-x86;win10-x64;win10-arm64</RuntimeIdentifiers>
<RuntimeIdentifiers>win-x86;win-x64;win-arm64</RuntimeIdentifiers>
```

## PublishProfileが警告を出してくる。
'Properties\PublishProfiles\win10-x64.pubxml' という名前の発行プロファイルがプロジェクトに見つかりませんでした。
```
    <PublishProfile>Properties\PublishProfiles\win10-$(Platform).pubxml</PublishProfile>
```
プロジェクトを右クリックして「発行」
ClickOnceProfile.pubxmlに変更したら、警告が消えたけど、そんなんで良いのだろうか？

## NETSDK1206	バージョン固有またはディストリビューション固有のランタイム識別子が見つかりました
指定された RID を必要としないことを把握している場合、これで警告を消す。
```
<PropertyGroup>
  <NoWarn>$(NoWarn);NETSDK1206</NoWarn>
</PropertyGroup>
```

# シェルページの仕組み
ShellPageは、アプリ起動時にActivationServiceからMainWindowにセットされる。  
ShellPageは、Frameを持っていて、Frameには、各ページがセットされる。  
ShellPageのxamlで各ページを表示する領域であるFrameを作成し、コードビハインドでそのFrameをページ表示場所として指定することで、各ページの表示を実現している。  
ShellPageのコードビハインドで、タイトルバーの設定や、キーボードショートカットの設定を行っている。

ここカスタマイズするの結構面倒臭いので、できるだけデフォルトのままで使用する。メニューを上にするか左にするかをプロジェクト作成前に決めておく。
ナビゲーションメニューはウィンドウを小さくすれば非表示になる。

# ローカライズ
Stringクラスに拡張メソッドがある。
$"{"AppDisplayName".GetLocalized()}" // AppDisplayNameのローカライズ

ファイルはStringsフォルダのen-usをコピーして、ja-jpフォルダを作成して、日本語版を記述するだけ。ただし、画面を追加したときは手動で追加すること。

# Humanizer
Humanizerライブラリが入っているので、Stringに対してメソッドを呼ぶことでパスカルケース化や、複数形化などの英単語変換ができる。

# 設定値の保存方法
## ユーザが触った設定値
ILocalSettingsServiceを注入して、キーを指定して値を保存する。
ユーザが編集した値として、アプリケーションの実行フォルダのLocalSettings.jsonに保存される。

## ユーザは触らない方の設定値
LocalSettingsOptionsは、設定値の保存先を指定するための列挙型で、設定内容はappsettings.jsonに記述される。ユーザは触らない。

## ファイルの保存方法について
FileServiceはMSIXを使用しない場合のみ使用し、従来の方法で読み書きを行う。このプロジェクトではこっちは使わない。この場合、"C:\Users\ユーザ名\AppData\Local"に保存される？

MSIXの場合、アプリケーションの実行フォルダは、C:\Users\ユーザ名\AppData\Local\Packages\アプリケーションID\LocalCache\LocalSettings.jsonに保存される。
その場合、ApplicationData.Current.LocalSettingsでアクセスする。  
このアプリは"83a7990a-84aa-479c-9662-45da248ee082_gcfy10y4r1fd6"がIDだった。（変わる？）

### MSIXでのファイルの扱い
- Windows.Storage.ApplicationData.Current.TemporaryFolder  
一時フォルダ。アプリケーションが終了すると削除される。
- Windows.ApplicationModel.Package.Current.InstalledLocation  
アプリのインストール先のフォルダ。読み取り専用。
- Windows.Storage.ApplicationData.Current.LocalFolder  
アプリ固有の永続的な保存領域。アプリのみが読み書きできる。
```csharp
var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
var file = await localFolder.CreateFileAsync("myfile.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
await FileIO.WriteTextAsync(file, "This is my file content.");
```
LocalSettingsServiceのデフォルトが結構酷いと思ったので、MSIXとの違いをなるべく吸収するように修正した。

### MSIXをやめたい場合
csprojの中に"EnableMsixTooling"があるのでFalseにする。
※MSIXだと1つのマシンに複数ユーザでインストールできない？全てのユーザで使えるようにインストールできる？


# トラブル
## PRI263: 0xdef01051
アプリケーションのビルド時に、PRI263: 0xdef01051というエラーが発生する場合があります。
Nugetで最新版に更新したら解消されました。
※それでも出たら？
https://learn.microsoft.com/ja-jp/windows/apps/windows-app-sdk/downloads
ここで最新版をインストールする。

## サブフォルダ追加して画面追加したら遷移できなかった
名前空間が違ったのが原因。また、xamlのナビゲーション引数も名前空間を反映していないのでキーが見つからなかった。
これは追加時に、実際のGit差分を見て確認すれば大丈夫。
名前空間が違う画面追加は想定されていないので注意すること。

## モードレスで表示したい
WinUI3では、モードレスウィンドウはサポートされていなかった。  
https://tera1707.com/entry/2022/06/14/231325  
WinUI3 Galleryのサンプルコードから、別ウインドウを開くためのヘルパー（WindowHelper）をそのままもらってきて、それを使ってウインドウを開く。  
あとは実際に表示する画面をPageで作成する。（Windowではない）  
ただし、このままでは元のウィンドウを閉じても連動して閉じられないっぽい。  
WindowHelperの中に、アプリが任意のUIElement（GetWindowForElement）を含むウィンドウを見つけられるようにするヘルパークラスがあるので、それを使えばいけそう。  

## SemanticKernelを入れたら、Newtonsoft.jsonが死んだ
System.Text.Jsonが使えるので、そちらに変更する。
単純置き換えで行ける。
Coreの中にあるHelpersのJsonは要らない。
```
// 修正前
JsonConvert.DeserializeObject<T>(value);
JsonConvert.SerializeObject(value);

// 修正後
JsonSerializer.Deserialize<T>(value);
return JsonSerializer.Serialize(value);
```
System.Text.JsonはWindows10の古いバージョンでは動かないため警告が出るらしい。

## ViewModelにEnumを送ったらintにされてしまう
ライブラリのバグのため、Microsoft.UI.XamlのEnumではない場合は、ViewModelにCommandParameterで渡したときにint型として受け付けることしかできなくなっている
https://github.com/CommunityToolkit/dotnet/discussions/407
https://github.com/microsoft/microsoft-ui-xaml/issues/7633
あと、デフォルトのEnumToBooleanConverterもおかしい。

## BoolToVisibilityConverterは、UserControlのUIを更新しません
https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/4777
```
Visibility="{x:Bind ViewModel.IsOpenAI, Converter={StaticResource BoolToVisibilityConverter}, Mode=OneWay}"
```
Mode=OneWayが必要ってだけです。気を付けよう。

## 各ページに対して設定したScrollViewまたはScrollViewerが出てこない。
ScrollViewを表示するには子要素の高さが表示領域を超えなければならないが、
ShellPageのデフォルトの書き方ではWindowからはみ出てもFrameが伸びるため超えない。
StackPanelでShellPageの要素を書いているため高さの制限が無いからである。
StackPanelをやめてGridのRowDefinitionを書いてやることで高さの制限ができてScrollViewerが出るようになる。

## CommandParameterにBindingが使えない
これは元々できないので、使わずに済む構造になるように作る。
