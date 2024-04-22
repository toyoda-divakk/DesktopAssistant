﻿# 作りたいもの
デスクトップアシスタントを作る。
キャラクターを作成してデスクトップに表示する。
キャラクターはAIを使って会話する。

## 直近の目標
- キャラクターの画像を表示する
- Todoリストを作成して、LiteDBで保存する。

# 機能
## キャラクターの設定
- キャラクターの画像を設定できる。
- キャラクターのプロフィールを記憶する。
- AIのプロンプトを記憶する。

## Todoリストの管理
- Todoリストを作成できる。
- TodoリストをAIに読ませて、コメントをしてもらえる。

# システム設定項目
- 使用するAIサービス  
AzureOpenAI, OpenAI, Claude
- APIキー

# キャラクター設定項目

# 作成するデータクラス
- キャラクター
- キャラクターとの会話履歴
- Todo

# 画面
## メイン
- 説明書を表示する
- 次にやることを表示する
## キャラクター一覧
## キャラクター詳細
## キャラクター設定
## Todoリスト一覧
- Todoリストの作成
## Todoリスト詳細
- Todoリストの編集
- Todoにコメントや進捗を付ける
- Todoにタグ（カテゴリ）を付ける
- リマインドの設定
## Todoリスト設定


# DBに保存する物
TODOリスト
キャラクターデータとその会話履歴


# やることリスト

## キャラクターサンプル
画像も保存するが、DBには保存しない。
データベースにはメタデータ(画像のタイトル、タグなど)のみを格納し、実際の画像ファイルのパスを参照する形式が一般的です。

ダミーの画像を用意する。
召喚ボタンを作って、キャラクターを表示する。

## チャット画面を作成する。
どうしようか？Semanticのサンプルを見る？それともゲームっぽくかわいい感じで？

## Todo表示画面の修正
Todo管理画面のボタンを考える。
左にメニューを作って、カテゴリ一覧を表示する。左下にカテゴリ追加ボタンを作る。

- 完了済みTODOの表示  
未完了のTODOだけでなく、完了済みのTODOも一覧で表示する  
完了済みのTODOは視覚的に区別できるようにする(グレーアウトや取り消し線など)
- 優先度の表示  
TODOに優先度(高/中/低)を設定できるようにする  
優先度によってTODOの表示順を変更したり、色分けするなどして視覚的に区別する
- 期限の表示  
TODOに期限を設定できるようにする  
期限が近づいているTODOを視覚的に強調する(色変更、アイコン表示など)
- 検索/絞り込み機能  
TODOの一覧を検索したり、優先度やステータス(完了/未完了)で絞り込めるようにする
- カテゴリ分類  
TODOをカテゴリ(仕事、家事、趣味など)で分類できるようにする  
カテゴリ毎にTODOを表示したり、カテゴリ間の移動ができると便利
- 統計情報の表示  
完了済みTODOの割合、優先度別の内訳など、TODOの状況を把握できる統計情報を表示する
- リマインダ機能  
TODOの期限が近づいたときに通知を受け取れるようにする
- バックアップ/復元  
TODOリストのデータをバックアップ/復元できる機能を提供する

# 使用イメージ
## 初回起動
## キャラ召喚以降の起動
初回起動画面はタスクトレイに収納し、前回召喚したキャラクターを表示する。
いつ切っても大丈夫なように、会話データなどは随時保存する。
