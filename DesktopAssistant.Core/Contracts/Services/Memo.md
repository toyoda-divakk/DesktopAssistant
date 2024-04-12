# 後で使う
```
// PCのタイムゾーンを取得
TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

// UTCをローカルのタイムゾーンに変換
DateTime utcDateTime = DateTime.UtcNow;
DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, localTimeZone);

// 日本のタイムゾーンを取得し、UTCを日本時間に変換
//TimeZoneInfo japanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
//DateTime japanDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, japanTimeZone);

Console.WriteLine($"UTC: {utcDateTime}");
Console.WriteLine($"Local Time: {localDateTime}");
Console.WriteLine($"Japan Time: {japanDateTime}");
```
TimeProviderを使うのもあり？