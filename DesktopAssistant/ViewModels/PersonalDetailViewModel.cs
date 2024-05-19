using CommunityToolkit.Mvvm.ComponentModel;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Contracts.ViewModels;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.ViewModels;

// TODO:キャラ・キャラクター → アシスタント

// 編集用の画面を別に作成する→GPT4-oに作ってもらおうか。

// この画面は、キャラクターの詳細を表示する画面
// 要素
// ・編集ボタン：編集画面に遷移
// ・削除ボタン：削除して前の画面に遷移
// ・キャラクター名
// ・キャラクターの画像
// ・キャラクターの説明
// ・キャラクターのプロンプト



public partial class PersonalDetailViewModel(INavigationService navigationService, ILiteDbService liteDbService) : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService = navigationService;
    private readonly ILiteDbService _liteDbService = liteDbService;

    [ObservableProperty]
    private Character? item;

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is long characterId)
        {
            Item = _liteDbService.GetTable<Character>().First(x => x.Id == characterId);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
// 編集画面下書き
//    <Window  
//    x:Class="YourNamespace.CharacterEditView"  
//    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"  
//    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"  
//    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"  
//    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"  
//    mc:Ignorable="d"  
//    Title="Edit Character">  
  

//<Grid>
//    <!-- Gridの列定義 -->
//    <Grid.ColumnDefinitions>
//        <ColumnDefinition Width="2*" />
//        <ColumnDefinition Width="3*" />
//    </Grid.ColumnDefinitions>

//    <!-- 左側の内容 -->
//    <StackPanel Grid.Column="0" Margin="10">
//        <TextBlock FontSize="24" FontWeight="Bold" Text="名前" />
//        <TextBox Text="{Binding Character.Name, Mode=TwoWay}" FontSize="20" />
//        <Image Source="{Binding Character.FaceImagePath}" Width="200" Height="200" Stretch="Uniform" />
//    </StackPanel>

//    <!-- 右側の内容 -->
//    <StackPanel Grid.Column="1" Margin="10">
//        <TextBlock FontSize="18" FontWeight="Bold" Text="見出し・説明" />
//        <TextBox Text="{Binding Character.Description, Mode=TwoWay}" Margin="0,5,0,15" FontSize="16" AcceptsReturn="True" TextWrapping="Wrap" />
//        <TextBlock FontSize="18" FontWeight="Bold" Text="プロンプト" />
//        <TextBox Text="{Binding Character.Prompt, Mode=TwoWay}" FontSize="16" />
//    </StackPanel>

//    <!-- 更新ボタン -->
//    <Button Content="更新" HorizontalAlignment="Right" VerticalAlignment="Bottom" Margin="10"  
//                Command="{Binding UpdateCommand}" Grid.ColumnSpan="2" Width="100" />
//</Grid>
//</Window>