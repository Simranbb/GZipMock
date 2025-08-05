using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGZipSample
{
    public interface IDateUtils
    {
        DateOnly Today { get; }

        string GetDateTimeInFormat(DateTime dateTime, string format);
        string GetDeviceDateTimeUTCInFormat(string format);
        string GetDeviceDateTimeInFormat(string format);
        DateTime GetDeviceDateInUTC();
        DateTime GetDeviceDate();
        int GetUnixTimeStamp(DateTime date);
        string GetBritishTimeWithOffset(DateTime utcTime);
        DateTime GetCutOffTime(string cutoffTime);
        DateTime ConvertUnixTimeStampToDateTime(double unixTimeStamp);
        bool IsLeapYear(int year);
        DateTime ParseDateTime(string dateTime, string[] formats);
        bool IsSunday();
    }
}
