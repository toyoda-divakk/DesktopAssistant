using System.Collections;
using System.Text;
using System.Text.Json;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Models;
using Humanizer;
using LiteDB;

namespace DesktopAssistant.Services;

/// <summary>
/// 時間に関する処理を行うサービス
/// </summary>
public class DateUtilService() : IDateUtilService
{
    public string GetRemainingTime(DateTime dateTime){
        var remainingTime = dateTime - DateTime.Now;
        if(remainingTime.TotalSeconds < 0){
            return "期限切れ";
        }
        if(remainingTime.TotalDays >= 1){
            return $"{remainingTime.Days}日";
        }
        return $"{remainingTime.Hours}時間";
    }
}

