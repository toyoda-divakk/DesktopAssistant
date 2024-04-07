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

Todo管理画面を作成する。

# DBに保存する物
TODOリスト
キャラクターデータ

# トラブル
## PRI263: 0xdef01051
アプリケーションのビルド時に、PRI263: 0xdef01051というエラーが発生する場合があります。
Nugetで最新版に更新したら解消されました。


