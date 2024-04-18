﻿using DesktopAssistant.Core.Models;

namespace DesktopAssistant.Core.Contracts.Services;

// サンプルデータを取得するサービスなので、自分のデータを作成するようになったら削除すること
public interface ISampleDataService
{
    IEnumerable<Character> GetSampleCharacters();
    IEnumerable<TodoTask> GetSampleTodoTasks();
}
