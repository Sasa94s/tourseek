using System;

namespace tourseek_backend.util.Extensions
{
    public static class DateUtils
    {
        public static DateTime CalulateStartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
    }
}
