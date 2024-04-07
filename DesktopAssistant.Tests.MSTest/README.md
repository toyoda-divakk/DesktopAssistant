*Recommended Markdown Viewer: [Markdown Editor](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor2)*

## はじめに

[ユニットテストを始める](https://docs.microsoft.com/visualstudio/test/getting-started-with-unit-testing?view=vs-2022&tabs=dotnet%2Cmstest)、 [ユニットテストで MSTest フレームワークを使う](https://docs.microsoft.com/visualstudio/test/using-microsoft-visualstudio-testtools-unittesting-members-in-unit-tests)、 [Test Explorer でユニットテストを実行する](https://docs.microsoft.com/visualstudio/test/run-unit-tests-with-test-explorer) は、 MSTest フレームワークと Test Explorer の概要を提供します。

## UI コントロールのテスト

UIコントロールを実行するユニットテストは、WinUI UIスレッドで実行しなければなりません。WinUI UIスレッドでテストを実行するには、テストメソッドに `[TestMethod]` の代わりに `[UITestMethod]` を付けてください。テスト実行中、テストホストはアプリを起動し、アプリのUIスレッドにテストをディスパッチします。

以下の例では `new Grid()` を作成し、その `ActualWidth` が `0` であることを検証している。

```csharp
[UITestMethod]
public void UITestMethod()
{
    Assert.AreEqual(0, new Grid().ActualWidth);
}
```

## 依存性の注入とモッキング

Template Studio では [依存性の注入](https://docs.microsoft.com/dotnet/core/extensions/dependency-injection) を使っています。これはクラスの依存性がインターフェイスを実装しており、クラスのコンストラクタ経由で依存性が注入されることを意味します。

この方法の多くの利点のひとつは、テストのしやすさが向上することです。テストはインターフェースのモック実装を作成し、それをテスト対象のオブジェクトに渡すことで、テスト対象のオブジェクトを依存関係から切り離すことができます。インターフェイスをモックするには、インターフェイスを実装したクラスを作成し、インターフェイスメンバのスタブ実装を作成し、そのクラスのインスタンスをオブジェクトのコンストラクタに渡します。

以下の例では、設定ページの ViewModel をテストしています。SettingsViewModel` は `IThemeSelectorService` に依存しているので、インターフェイスを実装した `MockThemeSelectorService` クラスをスタブ実装とともに導入し、そのクラスのインスタンスを `SettingsViewModel` のコンストラクタに渡す。VerifyVersionDescription` テストでは、`SettingsViewModel` の `VersionDescription` プロパティが期待通りの値を返すかどうかを検証する。

```csharp
// SettingsViewModelTests.cs

[TestClass]
public class SettingsViewModelTests
{
    private readonly SettingsViewModel _viewModel;

    public SettingsViewModelTests()
    {
        _viewModel = new SettingsViewModel(new MockThemeSelectorService());
    }

    [TestMethod]
    public void VerifyVersionDescription()
    {
        Assert.IsTrue(Regex.IsMatch(_viewModel.VersionDescription, @"App1 - \d\.\d\.\d\.\d"));
    }
}
```

```csharp
// Mocks/MockThemeSelectorService.cs

internal class MockThemeSelectorService : IThemeSelectorService
{
    public ElementTheme Theme => ElementTheme.Default;

    public Task InitializeAsync() => Task.CompletedTask;

    public Task SetRequestedThemeAsync() => Task.CompletedTask;

    public Task SetThemeAsync(ElementTheme theme) => Task.CompletedTask;
}
```

## CIパイプライン

CIパイプラインでのプロジェクトのビルドとテストについては、[README.md](https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/pipelines/README.md)を参照してください。
