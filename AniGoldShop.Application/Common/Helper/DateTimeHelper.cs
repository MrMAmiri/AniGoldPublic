using System;
using System.Collections.Generic;
using System.Text;

namespace AniGoldShop.Application.Common.Helper
{
    public static class DateTimeHelper
    {
        public static DateTime ToLocalDate(DateTime dateTime,TimeZoneInfo timeZone)
        {
            dateTime = UseAsUTCDateTime(dateTime);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone).Date;
        }

        public static DateTime ToLocalDate(DateTime date, TimeSpan time, TimeZoneInfo timeZone)
        {
            date = UseAsUTCDateTime(date).Date;
            return ToLocalDate(date.Add(time), timeZone);
        }

        public static TimeSpan ToLocalTime(DateTime dateTime, TimeZoneInfo timeZone)
        {
            dateTime = UseAsUTCDateTime(dateTime);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone).TimeOfDay;
        }

        public static TimeSpan ToLocalTime(DateTime date, TimeSpan time, TimeZoneInfo timeZone)
        {
            date = UseAsUTCDateTime(date).Date;
            return ToLocalTime(date.Add(time), timeZone);
        }

        public static DateTime ToLocalDateTime(DateTime dateTime, TimeZoneInfo timeZone)
        {
            dateTime = UseAsUTCDateTime(dateTime);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);
        }

        public static DateTime ToLocalDateTime(DateTime date, TimeSpan time, TimeZoneInfo timeZone)
        {
            date = UseAsUTCDateTime(date).Date;
            return ToLocalDateTime(date.Add(time), timeZone);
        }

        public static DateTime UseAsUTCDateTime(DateTime date)
        {
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;

            var hour = date.Hour;
            var minute = date.Minute;
            var second = date.Second;

            return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        }

        public static DateTime UseAsUTCDateTime(DateTime date, TimeSpan time)
        {
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;

            var hour = time.Hours;
            var minute = time.Minutes;
            var second = time.Seconds;

            return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        }
    }
}
