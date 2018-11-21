using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHEER.PresentationLayer.Extension
{
    public static class DateTimeExtension
    {
        public static string ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm");
        }

        public static int DateTimeToUnixTimestamp(this DateTime dateTime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(dateTime - startTime).TotalSeconds;
        }
    }
}