using System.Globalization;

namespace Base.Helpers;

public static class DateTimeExtensions
{
    /// <summary>
    /// Returns the first day of the week that the specified
    /// date is in using the current culture. 
    /// </summary>
    public static DateTime FirstDayOfWeek(this DateTime dayInWeek)
    {
        return FirstDayOfWeek(dayInWeek, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns the first day of the week that the specified date 
    /// is in. 
    /// </summary>
    public static DateTime FirstDayOfWeek(this DateTime dayInWeek, CultureInfo cultureInfo)
    {
        DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
        DateTime firstDayInWeek = dayInWeek.Date;
        while (firstDayInWeek.DayOfWeek != firstDay)
            firstDayInWeek = firstDayInWeek.AddDays(-1);

        return firstDayInWeek;
    }

    public static DateTime BeginningOfToday(this DateTime value)
    {
        var res = value
            .AddHours(-value.Hour)
            .AddMinutes(-value.Minute)
            .AddSeconds(-value.Second)
            .AddMilliseconds(-value.Millisecond)
            .AddMicroseconds(-value.Microsecond);
        return res;
    }

    public static DateTime EndOfToday(this DateTime value)
    {
        var res = value
            .AddHours(23 - value.Hour)
            .AddMinutes(59 - value.Minute)
            .AddSeconds(59 - value.Second)
            .AddMilliseconds(999 - value.Millisecond)
            .AddMicroseconds(999 - value.Microsecond);
        return res;
    }

    public static DateTime FirstDayOfMonth(this DateTime value)
    {
        return FirstDayOfMonth(value, 0);
    }

    public static DateTime FirstDayOfMonth(this DateTime value, int monthChangeAmount)
    {
        return new DateTime(value.Year, value.Month + monthChangeAmount, 1);
    }

    public static int DaysInMonth(this DateTime value)
    {
        return DateTime.DaysInMonth(value.Year, value.Month);
    }

    public static DateTime LastDayOfMonth(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, value.DaysInMonth());
    }

    public static DateTime LastDayWithTimeOfMonth(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, value.DaysInMonth(), 23, 59, 59);
    }
}