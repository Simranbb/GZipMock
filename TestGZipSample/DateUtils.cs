using System;
using System.Globalization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGZipSample
{
    public class DateUtils : IDateUtils
    {
        public DateOnly Today
            => DateOnly.FromDateTime(DateTime.Now);

        public string GetDeviceDateTimeUTCInFormat(string format)
        {
            return DateTime.UtcNow.ToString(format);
        }

        public string GetDeviceDateTimeInFormat(string format)
        {
            return DateTime.Now.ToString(format);
        }

        public DateTime GetDeviceDateInUTC()
        {
            return DateTime.UtcNow;
        }

        public DateTime GetDeviceDate()
        {
            return DateTime.Now;
        }

        public int GetUnixTimeStamp(DateTime date)
        {
            return (int)date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public string GetDateTimeInFormat(DateTime dateTime, string format)
        {
            return dateTime.ToString(format);
        }

        public string GetBritishTimeWithOffset(DateTime utcTime)
        {
            // Ensure that the given date and time is not a specific kind. 
            utcTime = DateTime.SpecifyKind(utcTime, DateTimeKind.Unspecified);

            var dtUtc = new DateTimeOffset(utcTime, TimeSpan.Zero);
            var britishZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var newDate = TimeZoneInfo.ConvertTime(dtUtc, britishZone);

            return newDate.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        public DateTime GetCutOffTime(string cutoffTime)
        {
            var currentTime = GetDeviceDate();
            var timespan = TimeSpan.Parse(cutoffTime);
            var cutOffTime = new DateTime(currentTime.Date.Year, currentTime.Date.Month, currentTime.Date.Day, timespan.Hours, timespan.Minutes, 0);
            return cutOffTime;
        }

        public DateTime ConvertUnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public bool IsLeapYear(int year)
            => DateTime.IsLeapYear(year);

        public DateTime ParseDateTime(string dateTime, string[] formats)
        {
            DateTime.TryParseExact(dateTime, formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsedDateTime);

            return parsedDateTime;
        }

        public bool IsSunday()
        {
            return GetDeviceDate().DayOfWeek is DayOfWeek.Sunday;
        }
    }
}
