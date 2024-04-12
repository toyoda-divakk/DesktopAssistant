# ローカライズ
Stringクラスに拡張メソッドがある。
$"{"AppDisplayName".GetLocalized()}" // AppDisplayNameのローカライズ

ファイルはStringsフォルダのen-usをコピーして、ja-jpフォルダを作成して、日本語版を記述するだけ。ただし、画面を追加したときは手動で追加すること。

# 設定値の保存方法
## ユーザが触った設定値
ILocalSettingsServiceを注入して、キーを指定して値を保存する。
ユーザが編集した値として、アプリケーションの実行フォルダのLocalSettings.jsonに保存される。

## ユーザは触らない方の設定値
LocalSettingsOptionsは、設定値の保存先を指定するための列挙型で、設定内容はappsettings.jsonに記述される。ユーザは触らない。


# やることリスト
https://learn.microsoft.com/ja-jp/windows/apps/design/style/segoe-fluent-icons-font

LiteDBを使ったNoSQLデータベースの作成を行い、ローカルファイルに保存する。
画像も保存するが、DBには保存しない。
データベースにはメタデータ(画像のタイトル、タグなど)のみを格納し、実際の画像ファイルのパスを参照する形式が一般的です。

編集ボタンを作って、データを編集できるようにする。

ダミーの画像を用意する。
召喚ボタンを作って、キャラクターを表示する。

デフォルトのデータを作成し、LiteDBで読み書きする。

●Todo管理画面を作成する。（画面の日付や完了の表示が不細工だが、後で直す）
Todo管理画面のボタンを考える。
DataGridだけど、カテゴリ別に折りたためないの？→ListViewの方が良いかもしれない。サンプルを見よう。
●データ構造を決定する。
●サンプルデータ作成。
もうちょっとマシなサンプルデータにする。
Chatクラスを作成する。

チャット画面を作成する。
とっととAzureOpenAIにつなぐ。


# DBに保存する物
TODOリスト
キャラクターデータ

# トラブル
## PRI263: 0xdef01051
アプリケーションのビルド時に、PRI263: 0xdef01051というエラーが発生する場合があります。
Nugetで最新版に更新したら解消されました。

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
単純置き換えで行けそう。
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
