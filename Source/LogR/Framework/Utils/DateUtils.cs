using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Utils
{
    public static class DateUtils
    {
        public readonly static DateTime InvalidDate = new DateTime(1900, 1, 1);

        public static bool IsInvalidDate(this DateTime? dt)
        {
            return ((dt == null) || (dt == InvalidDate) || (dt == DateTime.MinValue) || (dt == DateTime.MaxValue));
        }

        public static bool IsInvalidDate(this DateTime dt)
        {
            return ((dt == null) || (dt == InvalidDate) || (dt == DateTime.MinValue) || (dt == DateTime.MaxValue));
        }

        public static bool IsValidDate(this DateTime? dt)
        {
            return !IsInvalidDate(dt);
        }

        public static bool IsValidDate(this DateTime dt)
        {
            return !IsInvalidDate(dt);
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

    }
}
