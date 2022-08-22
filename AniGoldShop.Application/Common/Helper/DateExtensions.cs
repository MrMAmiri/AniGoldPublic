using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application.Common.Helper
{
    public static class DateTimeExtensions
    {
        public static string ToMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }
        public static string ToMonthName(this DateTime? dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)(dateTime?.Month));
        }


        public static string ToShortMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
        }

        public static DateTime? ToGeorgDate(this string val)
        {

            if (string.IsNullOrWhiteSpace(val))
                return null;

            try
            {
                return Convert.ToDateTime(Convert.ToDateTime(val, new CultureInfo("fa-IR")), new CultureInfo("en-US"));
            }
            catch
            {
                return null;
            }
        }
        public static DateTime? ToPersianDate(this DateTime val)
        {
            try
            {
                return Convert.ToDateTime(val, new CultureInfo("fa-IR"));
            }
            catch
            {
                return null;
            }
        }

        public static string ToPersianDateString(this DateTime val, bool withTime = false)
        {
            try
            {
                if (!withTime)
                    return val.ToString("yyyy/MM/dd", new CultureInfo("fa-IR"));
                else
                    return val.ToString("yyyy/MM/dd HH:mm", new CultureInfo("fa-IR"));
            }
            catch
            {
                return null;
            }
        }
    }
}
