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
    public IEnumerable<Character> GetSampleCharacters()
    {
        var characterId1 = 1;
        var characterId2 = 2;
        var characterId3 = 3;
        var characterId4 = 4;
        return
        [
            new()
            {
                Id = characterId1,
                Order = characterId1,
                Name = "ドンペン",
                Description = "厳しい口調で話します",
                Prompt = "とかげ",
                BackColor = "#202020",
                TextColor = "#ffffff",
                Topics =
                [
                    new()
                    {
                        Id = 1,
                        CharacterId = characterId1,
                        Subject = "朝の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 1,
                                IsCharacterMessage = false,
                                Text = "おはよう",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00)
                            },
                            new()
                            {
                                TopicId = 1,
                                IsCharacterMessage = true,
                                Text = "何だ貴様は",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00)
                            },
                            new()
                            {
                                TopicId = 1,
                                IsCharacterMessage = false,
                                Text = "ああああ",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00)
                            },
                            new()
                            {
                                TopicId = 1,
                                IsCharacterMessage = true,
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
                        CharacterId = characterId1,
                        Subject = "昼の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 2,
                                IsCharacterMessage = false,
                                Text = "こんにちは",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00)
                            },
                            new()
                            {
                                TopicId = 2,
                                IsCharacterMessage = true,
                                Text = "おう、今日は何の用だ？",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00)
                            },
                            new()
                            {
                                TopicId = 2,
                                IsCharacterMessage = false,
                                Text = "うううう",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00)
                            },
                            new()
                            {
                                TopicId = 2,
                                IsCharacterMessage = true,
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
                Id = characterId2,
                Order = characterId2,
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
                        CharacterId = characterId2,
                        Subject = "朝の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 3,
                                IsCharacterMessage = false,
                                Text = "おはよう",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00)
                            },
                            new()
                            {
                                TopicId = 3,
                                IsCharacterMessage = true,
                                Text = "何だ貴様は",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00)
                            },
                            new()
                            {
                                TopicId = 3,
                                IsCharacterMessage = false,
                                Text = "ああああ",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00)
                            },
                            new()
                            {
                                TopicId = 3,
                                IsCharacterMessage = true,
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
                        CharacterId = characterId2,
                        Subject = "昼の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 4,
                                IsCharacterMessage = false,
                                Text = "こんにちは",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00)
                            },
                            new()
                            {
                                TopicId = 4,
                                IsCharacterMessage = true,
                                Text = "おう、今日は何の用だ？",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00)
                            },
                            new()
                            {
                                TopicId = 4,
                                IsCharacterMessage = false,
                                Text = "うううう",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00)
                            },
                            new()
                            {
                                TopicId = 4,
                                IsCharacterMessage = true,
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
                Id = characterId3,
                Order = characterId3,
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
                        CharacterId = characterId3,
                        Subject = "朝の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 5,
                                IsCharacterMessage = false,
                                Text = "おはよう",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00)
                            },
                            new()
                            {
                                TopicId = 5,
                                IsCharacterMessage = true,
                                Text = "何だ貴様は",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00)
                            },
                            new()
                            {
                                TopicId = 5,
                                IsCharacterMessage = false,
                                Text = "ああああ",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00)
                            },
                            new()
                            {
                                TopicId = 5,
                                IsCharacterMessage = true,
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
                        CharacterId = characterId3,
                        Subject = "昼の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 6,
                                IsCharacterMessage = false,
                                Text = "こんにちは",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00)
                            },
                            new()
                            {
                                TopicId = 6,
                                IsCharacterMessage = true,
                                Text = "おう、今日は何の用だ？",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00)
                            },
                            new()
                            {
                                TopicId = 6,
                                IsCharacterMessage = false,
                                Text = "うううう",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00)
                            },
                            new()
                            {
                                TopicId = 6,
                                IsCharacterMessage = true,
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
                Id = characterId4,
                Order = characterId4,
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
                        CharacterId = characterId4,
                        Subject = "朝の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 7,
                                IsCharacterMessage = false,
                                Text = "おはよう",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00)
                            },
                            new()
                            {
                                TopicId = 7,
                                IsCharacterMessage = true,
                                Text = "何だ貴様は",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00)
                            },
                            new()
                            {
                                TopicId = 7,
                                IsCharacterMessage = false,
                                Text = "ああああ",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00)
                            },
                            new()
                            {
                                TopicId = 7,
                                IsCharacterMessage = true,
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
                        CharacterId = characterId3,
                        Subject = "昼の挨拶",
                        Messages =
                        [
                            new()
                            {
                                TopicId = 8,
                                IsCharacterMessage = false,
                                Text = "こんにちは",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00)
                            },
                            new()
                            {
                                TopicId = 8,
                                IsCharacterMessage = true,
                                Text = "おう、今日は何の用だ？",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00)
                            },
                            new()
                            {
                                TopicId = 8,
                                IsCharacterMessage = false,
                                Text = "うううう",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00)
                            },
                            new()
                            {
                                TopicId = 8,
                                IsCharacterMessage = true,
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
