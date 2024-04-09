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
                Chats = new List<Chat>()
                {
                    new()
                    {
                        Id = 10001,
                        CharacterId = characterId1, // TODO:ここをnullにするんだけど、そのためにTopicクラスを作る必要がある
                        Text = "おはよう",
                        CreatedAt = new DateTime(1997, 9, 22, 9, 10, 00),
                    },
                    new()
                    {
                        Id = 10002,
                        CharacterId = characterId1,
                        Text = "何だ貴様は",
                        CreatedAt = new DateTime(1997, 9, 22, 9, 11, 00),
                    },
                    new()
                    {
                        Id = 10003,
                        CharacterId = characterId1,
                        Text = "ああああ",
                        CreatedAt = new DateTime(1997, 9, 22, 9, 12, 00),
                    },
                    new()
                    {
                        Id = 10007,
                        CharacterId = characterId1,
                        Text = "いいいい",
                        CreatedAt = new DateTime(1997, 9, 22, 9, 13, 00),
                    }
                }
            },
            new()
            {
                Id = characterId2,
                Name = "プランク",
                Prompt = "種族未定",
                Chats = new List<Chat>()
                {
                    new()
                    {
                        Id = 10004,
                        CharacterId = characterId2,
                        Text = "こんにちは",
                        CreatedAt = new DateTime(1997, 9, 22, 10, 10, 00),
                    },
                    new()
                    {
                        Id = 10005,
                        CharacterId = characterId2,
                        Text = "おう、今日は何の用だ？",
                        CreatedAt = new DateTime(1997, 9, 22, 10, 11, 00),
                    },
                    new()
                    {
                        Id = 10006,
                        CharacterId = characterId2,
                        Text = "うううう",
                        CreatedAt = new DateTime(1997, 9, 22, 10, 12, 00),
                    },
                    new()
                    {
                        Id = 10008,
                        CharacterId = characterId2,
                        Text = "ええええ",
                        CreatedAt = new DateTime(1997, 9, 22, 10, 13, 00),
                    }
                }
            },
            new()
            {
                Id = characterId3,
                Name = "カルモン",
                Prompt = "にわとり",
                Chats = new List<Chat>()
                {
                    new()
                    {
                        Id = 10009,
                        CharacterId = characterId3,
                        Text = "こんばんは",
                        CreatedAt = new DateTime(1997, 9, 22, 11, 10, 00),
                    },
                    new()
                    {
                        Id = 10010,
                        CharacterId = characterId3,
                        Text = "おう、お前か。",
                        CreatedAt = new DateTime(1997, 9, 22, 11, 11, 00),
                    },
                    new()
                    {
                        Id = 10011,
                        CharacterId = characterId3,
                        Text = "おおおお",
                        CreatedAt = new DateTime(1997, 9, 22, 11, 12, 00),
                    },
                    new()
                    {
                        Id = 10012,
                        CharacterId = characterId3,
                        Text = "かかかか",
                        CreatedAt = new DateTime(1997, 9, 22, 11, 13, 00),
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
                Title = "タスク一覧を見る",
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
                Title = "タスク一覧を見る",
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
