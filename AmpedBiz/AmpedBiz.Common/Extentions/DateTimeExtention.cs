using System;

namespace AmpedBiz.Common.Extentions
{
    public static class DateTimeExtention
    {
        public static double MonthDurationUntil(this DateTimeOffset thisDate, DateTimeOffset thatDate)
        {
            return Math.Abs((thisDate.Month - thatDate.Month) + 12 * (thisDate.Year - thatDate.Year));
        }

        public static double WeekDurationUntil(this DateTimeOffset thisDate, DateTimeOffset thatDate)
        {
            return Math.Abs((thisDate - thatDate).TotalDays) / 7;
        }

        public static double WeekDayDurationUntil(this DateTimeOffset thisDate, DateTimeOffset thatDate)
        {
            var days = Math.Abs((thisDate - thatDate).TotalDays) + 1;
            return ((days / 7) * 5) + (days % 7);
        }

        public static double DayDurationUntil(this DateTimeOffset thisDate, DateTimeOffset thatDate)
        {
            return Math.Abs((thisDate - thatDate).TotalDays);
        }

        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return dateTime.Date.AddDays(1).AddTicks(-1);
        }
    }
}
