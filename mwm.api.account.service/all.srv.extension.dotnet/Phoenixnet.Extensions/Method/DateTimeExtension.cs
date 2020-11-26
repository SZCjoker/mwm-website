using System;

namespace Phoenixnet.Extensions.Method
{
    public static class DateTimeExtension
    {
        public static string ToyyyyMM(this DateTimeOffset datetime)
        {
            return datetime.ToString("yyyyMM");
        }

        public static string ToyyyyMMdd(this DateTimeOffset datetime)
        {
            return datetime.ToString("yyyyMMdd");
        }

        public static string ToyyyyMMddHHmmss(this DateTimeOffset datetime)
        {
            return datetime.ToString("yyyyMMddHHmmss");
        }

        public static string ToyyyyMM(this DateTime datetime)
        {
            return datetime.ToString("yyyyMM");
        }

        public static string ToyyyyMMdd(this DateTime datetime)
        {
            return datetime.ToString("yyyyMMdd");
        }

        public static string ToyyyyMMddHHmmss(this DateTime datetime)
        {
            return datetime.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Returns the number of seconds that have elapsed since 1970-01-01T00:00:00Z.
        /// </summary>
        /// <param name="dateTime">Datetime value.</param>
        /// <returns>The number of seconds that have elapsed since 1970-01-01T00:00:00Z.</returns>
        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            DateTimeOffset offset = dateTime;
            return offset.ToUnixTimeSeconds();
        }
    }
}