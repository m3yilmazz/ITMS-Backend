using System;

namespace ITMSWebAPI.Utility
{
    public static class DateTimeExtension
    {
        public static long ToUnixTime(this DateTime input)
        {
            TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local));

            return Convert.ToInt64((input - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).Add(TimeSpan.FromHours(utcOffset.Hours * -1))
                .TotalSeconds);
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            TimeZoneInfo tz = TimeZoneInfo.Local;
            TimeSpan utcOffset = tz.GetUtcOffset(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local));

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).Add(utcOffset);

            var returnDatetime = dtDateTime.AddHours(3).AddSeconds(unixTime).ToLocalTime();

            return returnDatetime;
        }
    }
}
