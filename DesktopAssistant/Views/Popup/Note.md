# Popupのページ追加メモ
TemplateStudioでPopupのページを追加した場合、以下の手順を追加で行う。

- ShellPage.xamlに追加されたページをコメントアウトする
- 作成したxamlをPopupフォルダに移して名前空間を変更する（フォルダ分けしないならやらなくてOK）
- 画面を開くコマンドからWindowHelper.CreateWindow()して、Window.ContentにそのPageをnewする。

※初めてPopupページ作った時、PageServiceでViewModel取れなかった気がしたけど何を直したか忘れた。2回目以降は不要。