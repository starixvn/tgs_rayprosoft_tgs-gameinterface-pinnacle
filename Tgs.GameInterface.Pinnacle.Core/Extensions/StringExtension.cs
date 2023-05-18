namespace Tgs.GameInterface.Pinnacle.Core.Extensions;

public static class StringExtension
{
    public static string NoLongerThan(this string str, int chars)
        => string.IsNullOrEmpty(str) || str.Length <= chars ? str : str[..chars];

    public static string ToDateTimeString(this DateTimeOffset dateTime)
    {
        var timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "eastern time", baseUtcOffset: TimeSpan.FromHours(-4), "eastern time", "eastern time");
        return TimeZoneInfo.ConvertTime(dateTime.ToUniversalTime(), timeZone).ToString("yyyy-MM-dd HH:mm:ss");
    }
}
