using System.ComponentModel;
using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.Core.Services;

/// <summary>
/// サンプルデータを保持します。
/// ※DBに入れるので保持は必要なくなる
/// </summary>
public class SampleDataService : ISampleDataService
{
    public IEnumerable<Assistant> GetSampleAssistants()
    {
        var assistantId1 = 1;
        var assistantId2 = 2;
        var assistantId3 = 3;
        var assistantId4 = 4;
        return
        [
            new()
            {
                IsSelected = true,
                Id = assistantId1,
                Order = assistantId1,
                Name = "ドンペン",
                Description = "厳しい口調で話します",
                Prompt = "You are a programmer who is familiar with C#, WPF, WinUI3, and CommunityToolkit. You help people with their programming tasks.",
                BackColor = "#202020",
                TextColor = "#ffffff",
                Topics =
                [
                    new()
                    {
                        Id = 1,
                        AssistantId = assistantId1,
                        Subject = "朝の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 1,
                                IsAssistantMessage = false,
                                Text = "おはよう",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00)
                            },
                            new()
                            {
                                TopicId = 1,
                                IsAssistantMessage = true,
                                Text = "何だ貴様は",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00)
                            },
                            new()
                            {
                                TopicId = 1,
                                IsAssistantMessage = false,
                                Text = "ああああ",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00)
                            },
                            new()
                            {
                                TopicId = 1,
                                IsAssistantMessage = true,
                                Text = "いいいい",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                            }
                        ],
                        CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                    },
                    new()
                    {
                        Id = 2,
                        AssistantId = assistantId1,
                        Subject = "昼の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 2,
                                IsAssistantMessage = false,
                                Text = "こんにちは",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00)
                            },
                            new()
                            {
                                TopicId = 2,
                                IsAssistantMessage = true,
                                Text = "おう、今日は何の用だ？",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00)
                            },
                            new()
                            {
                                TopicId = 2,
                                IsAssistantMessage = false,
                                Text = "うううう",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00)
                            },
                            new()
                            {
                                TopicId = 2,
                                IsAssistantMessage = true,
                                Text = "ええええ",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                            }
                        ],
                        CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                    }
                ]
            },
            new()
            {
                IsSelected = false,
                Id = assistantId2,
                Order = assistantId2,
                Name = "プランク",
                Description = "やんちゃな口調で話します",
                Prompt = "種族未定",
                BackColor = "#ffffc6",
                TextColor = "#000000",
                Topics =
                [
                    new()
                    {
                        Id = 3,
                        AssistantId = assistantId2,
                        Subject = "朝の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 3,
                                IsAssistantMessage = false,
                                Text = "おはよう",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00)
                            },
                            new()
                            {
                                TopicId = 3,
                                IsAssistantMessage = true,
                                Text = "何だ貴様は",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00)
                            },
                            new()
                            {
                                TopicId = 3,
                                IsAssistantMessage = false,
                                Text = "ああああ",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00)
                            },
                            new()
                            {
                                TopicId = 3,
                                IsAssistantMessage = true,
                                Text = "いいいい",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                            }
                        ],
                        CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                    },
                    new()
                    {
                        Id = 4,
                        AssistantId = assistantId2,
                        Subject = "昼の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 4,
                                IsAssistantMessage = false,
                                Text = "こんにちは",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00)
                            },
                            new()
                            {
                                TopicId = 4,
                                IsAssistantMessage = true,
                                Text = "おう、今日は何の用だ？",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00)
                            },
                            new()
                            {
                                TopicId = 4,
                                IsAssistantMessage = false,
                                Text = "うううう",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00)
                            },
                            new()
                            {
                                TopicId = 4,
                                IsAssistantMessage = true,
                                Text = "ええええ",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                            }
                        ],
                        CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                    }
                ]
            },
            new()
            {
                IsSelected = false,
                Id = assistantId3,
                Order = assistantId3,
                Name = "カルモン",
                Description = "優しく話します",
                Prompt = "にわとり",
                BackColor = "#ff9e9e",
                TextColor = "#000000",
                Topics =
                [
                    new()
                    {
                        Id = 5,
                        AssistantId = assistantId3,
                        Subject = "朝の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 5,
                                IsAssistantMessage = false,
                                Text = "おはよう",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00)
                            },
                            new()
                            {
                                TopicId = 5,
                                IsAssistantMessage = true,
                                Text = "何だ貴様は",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00)
                            },
                            new()
                            {
                                TopicId = 5,
                                IsAssistantMessage = false,
                                Text = "ああああ",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00)
                            },
                            new()
                            {
                                TopicId = 5,
                                IsAssistantMessage = true,
                                Text = "いいいい",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                            }
                        ],
                        CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                    },
                    new()
                    {
                        Id = 6,
                        AssistantId = assistantId3,
                        Subject = "昼の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 6,
                                IsAssistantMessage = false,
                                Text = "こんにちは",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00)
                            },
                            new()
                            {
                                TopicId = 6,
                                IsAssistantMessage = true,
                                Text = "おう、今日は何の用だ？",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00)
                            },
                            new()
                            {
                                TopicId = 6,
                                IsAssistantMessage = false,
                                Text = "うううう",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00)
                            },
                            new()
                            {
                                TopicId = 6,
                                IsAssistantMessage = true,
                                Text = "ええええ",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                            }
                        ],
                        CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                    }
                ]
            },
            new()
            {
                IsSelected = false,
                Id = assistantId4,
                Order = assistantId4,
                Name = "ツン子",
                Description = "ツンデレです",
                Prompt = "にんげん",
                BackColor = "#ffc1c1",
                TextColor = "#000000",
                Topics =
                [
                    new()
                    {
                        Id = 7,
                        AssistantId = assistantId4,
                        Subject = "朝の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 7,
                                IsAssistantMessage = false,
                                Text = "おはよう",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00)
                            },
                            new()
                            {
                                TopicId = 7,
                                IsAssistantMessage = true,
                                Text = "何だ貴様は",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00)
                            },
                            new()
                            {
                                TopicId = 7,
                                IsAssistantMessage = false,
                                Text = "ああああ",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00)
                            },
                            new()
                            {
                                TopicId = 7,
                                IsAssistantMessage = true,
                                Text = "いいいい",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                            }
                        ],
                        CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                    },
                    new()
                    {
                        Id = 8,
                        AssistantId = assistantId3,
                        Subject = "昼の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 8,
                                IsAssistantMessage = false,
                                Text = "こんにちは",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00)
                            },
                            new()
                            {
                                TopicId = 8,
                                IsAssistantMessage = true,
                                Text = "おう、今日は何の用だ？",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00)
                            },
                            new()
                            {
                                TopicId = 8,
                                IsAssistantMessage = false,
                                Text = "うううう",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00)
                            },
                            new()
                            {
                                TopicId = 8,
                                IsAssistantMessage = true,
                                Text = "ええええ",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                            }
                        ],
                        CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                    }
                ]
            }
        ];
    }

    public IEnumerable<TaskCategory> GetSampleTodoTasks()
    {
        var categoryId1 = 1;
        var categoryId2 = 2;
        return
        [
            new (){
                Id = categoryId1,
                Order = categoryId1,
                Name = "ソフトの使い方",
                TodoTasks =
                [
                    new()
                    {
                        Title = "タスク一覧を見る",
                        CategoryId = categoryId1,
                        Order = 1,
                        Content = "今どんなタスクが登録されているか見てみよう。",
                        Progress = "進捗メモがあればここに書いてね。",
                        IsDone = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Deadline = DateTime.UtcNow + TimeSpan.FromDays(10)
                    },
                    new()
                    {
                        Title = "アシスタント一覧を見る",
                        CategoryId = categoryId1,
                        Order = 2,
                        Content = "今どんなアシスタントが登録されているか見てみよう。",
                        Progress = "メニューからアシスタント一覧ボタンをクリックします。",
                        IsDone = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Deadline = DateTime.UtcNow + TimeSpan.FromDays(10)
                    },
                    new()
                    {
                        Title = "OpenAI(ChatGPT)のAPIキーを手に入れる",
                        CategoryId = categoryId1,
                        Order = 3,
                        Content = "OpenAIにアカウントを登録しよう。",
                        Progress = "AIを利用するには、OpenAIかAzureのどちらかでAPIキーを作成する必要があります。",
                        IsDone = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Deadline = DateTime.UtcNow + TimeSpan.FromDays(10)
                    },
                    new()
                    {
                        Title = "AzureのAPIキーを手に入れる",
                        CategoryId = categoryId1,
                        Order = 4,
                        Content = "AzureでOpenAIサービスを登録しよう。",
                        Progress = "AIを利用するには、OpenAIかAzureのどちらかでAPIキーを作成する必要があります。",
                        IsDone = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Deadline = DateTime.UtcNow + TimeSpan.FromDays(10)
                    },
                    new()
                    {
                        Title = "APIの設定をする",
                        CategoryId = categoryId1,
                        Order = 5,
                        Content = "作成したAPIキーを設定画面に入力します。",
                        Progress = "メニューから設定ボタンをクリックします。",
                        IsDone = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Deadline = DateTime.UtcNow + TimeSpan.FromDays(10)
                    }
                ]
            },
            new (){
                Id = categoryId2,
                Order = categoryId2,
                Name = "自由",
                TodoTasks =
                [
                    new()
                    {
                        Title = "タスクの登録",
                        CategoryId = categoryId2,
                        Order = 1,
                        Content = "タスクを登録してみよう。",
                        Progress = "",
                        IsDone = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Deadline = DateTime.UtcNow + TimeSpan.FromDays(10)
                    }
                ]
            },
        ];
    }
}
