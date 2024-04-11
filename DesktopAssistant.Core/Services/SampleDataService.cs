using DesktopAssistant.Core.Contracts.Services;
using DesktopAssistant.Core.Models;

namespace DesktopAssistant.Core.Services;

/// <summary>
/// サンプルデータを保持します。
/// </summary>
public class SampleDataService : ISampleDataService
{
    private List<Character> _allCharacters;
    private List<TodoTask> _allTodoTasks;

    //private static IEnumerable<SampleOrder> AllOrders()
    //{
    //    var companies = AllCompanies();
    //    return companies.SelectMany(c => c.Orders);
    //}

    private static IEnumerable<Character> AllCharacters()
    {
        var characterId1 = 10001;
        var characterId2 = 10002;
        var characterId3 = 10003;
        return new List<Character>()
        {
            new()
            {
                Id = characterId1,
                Name = "ドンペン",
                Prompt = "とかげ",
                Topics = new List<Topic>()
                {
                    new()
                    {
                        Id = 1,
                        CharacterId = characterId1,
                        Subject = "朝の挨拶",
                        Messages = new List<Message>()
                        {
                            new()
                            {
                                Id = 10001,
                                TopicId = 1,
                                IsCharacterMessage = false,
                                Text = "おはよう",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00)
                            },
                            new()
                            {
                                Id = 10002,
                                TopicId = 1,
                                IsCharacterMessage = true,
                                Text = "何だ貴様は",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00)
                            },
                            new()
                            {
                                Id = 10003,
                                TopicId = 1,
                                IsCharacterMessage = false,
                                Text = "ああああ",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00)
                            },
                            new()
                            {
                                Id = 10004,
                                TopicId = 1,
                                IsCharacterMessage = true,
                                Text = "いいいい",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                            }
                        },
                        CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                    },
                    new()
                    {
                        Id = 2,
                        CharacterId = characterId1,
                        Subject = "昼の挨拶",
                        Messages = new List<Message>()
                        {
                            new()
                            {
                                Id = 10005,
                                TopicId = 2,
                                IsCharacterMessage = false,
                                Text = "こんにちは",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00)
                            },
                            new()
                            {
                                Id = 10006,
                                TopicId = 2,
                                IsCharacterMessage = true,
                                Text = "おう、今日は何の用だ？",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00)
                            },
                            new()
                            {
                                Id = 10007,
                                TopicId = 2,
                                IsCharacterMessage = false,
                                Text = "うううう",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00)
                            },
                            new()
                            {
                                Id = 10008,
                                TopicId = 2,
                                IsCharacterMessage = true,
                                Text = "ええええ",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                            }
                        },
                        CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                    }
                }
            },
            new()
            {
                Id = characterId2,
                Name = "プランク",
                Prompt = "種族未定",
                Topics = new List<Topic>()
                {
                    new()
                    {
                        Id = 3,
                        CharacterId = characterId2,
                        Subject = "朝の挨拶",
                        Messages = new List<Message>()
                        {
                            new()
                            {
                                Id = 10009,
                                TopicId = 3,
                                IsCharacterMessage = false,
                                Text = "おはよう",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00)
                            },
                            new()
                            {
                                Id = 10010,
                                TopicId = 3,
                                IsCharacterMessage = true,
                                Text = "何だ貴様は",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00)
                            },
                            new()
                            {
                                Id = 10011,
                                TopicId = 3,
                                IsCharacterMessage = false,
                                Text = "ああああ",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00)
                            },
                            new()
                            {
                                Id = 10012,
                                TopicId = 3,
                                IsCharacterMessage = true,
                                Text = "いいいい",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                            }
                        },
                        CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                    },
                    new()
                    {
                        Id = 4,
                        CharacterId = characterId2,
                        Subject = "昼の挨拶",
                        Messages = new List<Message>()
                        {
                            new()
                            {
                                Id = 10013,
                                TopicId = 4,
                                IsCharacterMessage = false,
                                Text = "こんにちは",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00)
                            },
                            new()
                            {
                                Id = 10014,
                                TopicId = 4,
                                IsCharacterMessage = true,
                                Text = "おう、今日は何の用だ？",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00)
                            },
                            new()
                            {
                                Id = 10015,
                                TopicId = 4,
                                IsCharacterMessage = false,
                                Text = "うううう",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00)
                            },
                            new()
                            {
                                Id = 10016,
                                TopicId = 4,
                                IsCharacterMessage = true,
                                Text = "ええええ",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                            }
                        },
                        CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                    }
                }
            },
            new()
            {
                Id = characterId3,
                Name = "カルモン",
                Prompt = "にわとり",
                Topics = new List<Topic>()
                {
                    new()
                    {
                        Id = 5,
                        CharacterId = characterId3,
                        Subject = "朝の挨拶",
                        Messages = new List<Message>()
                        {
                            new()
                            {
                                Id = 10017,
                                TopicId = 5,
                                IsCharacterMessage = false,
                                Text = "おはよう",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00)
                            },
                            new()
                            {
                                Id = 10018,
                                TopicId = 5,
                                IsCharacterMessage = true,
                                Text = "何だ貴様は",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00)
                            },
                            new()
                            {
                                Id = 10019,
                                TopicId = 5,
                                IsCharacterMessage = false,
                                Text = "ああああ",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00)
                            },
                            new()
                            {
                                Id = 10020,
                                TopicId = 5,
                                IsCharacterMessage = true,
                                Text = "いいいい",
                                CreatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                            }
                        },
                        CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 9, 13, 00)
                    },
                    new()
                    {
                        Id = 6,
                        CharacterId = characterId3,
                        Subject = "昼の挨拶",
                        Messages = new List<Message>()
                        {
                            new()
                            {
                                Id = 10021,
                                TopicId = 6,
                                IsCharacterMessage = false,
                                Text = "こんにちは",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00)
                            },
                            new()
                            {
                                Id = 10022,
                                TopicId = 6,
                                IsCharacterMessage = true,
                                Text = "おう、今日は何の用だ？",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00)
                            },
                            new()
                            {
                                Id = 10023,
                                TopicId = 6,
                                IsCharacterMessage = false,
                                Text = "うううう",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00)
                            },
                            new()
                            {
                                Id = 10024,
                                TopicId = 6,
                                IsCharacterMessage = true,
                                Text = "ええええ",
                                CreatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                            }
                        },
                        CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00),
                        UpdatedAt = new DateTime(1997, 9, 22, 10, 13, 00)
                    }
                }
            }
        };
    }

    private static IEnumerable<TodoTask> AllTodoTasks()
    {
        return new List<TodoTask>()
        {
            new()
            {
                Id = 1,
                Title = "タスク一覧を見る1",
                Content = "今どんなタスクが登録されているか見てみよう。",
                Progress = "特になし",
                IsDone = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(10)
            },
            new()
            {
                Id = 2,
                Title = "タスク一覧を見る2",
                Content = "今どんなタスクが登録されているか見てみよう。",
                Progress = "特になし",
                IsDone = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(10)
            }
        };
    }

    public async Task<IEnumerable<Character>> GetCharacterDataAsync()
    {
        _allCharacters ??= new List<Character>(AllCharacters());

        await Task.CompletedTask;
        return _allCharacters;
    }

    public async Task<IEnumerable<TodoTask>> GetTodoTaskDataAsync()
    {
        _allTodoTasks ??= new List<TodoTask>(AllTodoTasks());

        await Task.CompletedTask;
        return _allTodoTasks;
    }
}
