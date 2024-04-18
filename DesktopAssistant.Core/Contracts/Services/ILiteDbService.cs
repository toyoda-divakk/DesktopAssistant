//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DesktopAssistant.Core.Contracts.Interfaces;
//using DesktopAssistant.Core.Models;
//using LiteDB;

//namespace DesktopAssistant.Core.Contracts.Services;

///// <summary>
///// とりあえずLiteDBを使ってみる
///// アプリ固有の操作はなるべく実装しないように
///// </summary>
//public interface ILiteDbService
//{
//    List<TodoTask> Test(string localPath);

//    /// <summary>
//    /// データファイルが存在しているか
//    /// </summary>
//    /// <param name="localPath">データフォルダのパス</param>
//    /// <returns></returns>
//    bool IsExistDatabase(string localPath);

//    /// <summary>
//    /// データファイルが無ければ作成
//    /// あれば削除して再作成
//    /// </summary>
//    /// <param name="localPath"></param>
//    void CreateOrInitializeDatabase(string localPath);
//}
