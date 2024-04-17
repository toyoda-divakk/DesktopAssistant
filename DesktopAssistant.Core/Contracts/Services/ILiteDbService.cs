using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopAssistant.Core.Contracts.Interfaces;
using DesktopAssistant.Core.Models;
using LiteDB;

namespace DesktopAssistant.Core.Contracts.Services;

/// <summary>
/// とりあえずLiteDBを使ってみる
/// </summary>
public interface ILiteDbService
{
    List<TodoTask> Test(string localPath);
}
