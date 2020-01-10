using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Utils
{
    public static class Utils
    {
        public static Int64 ToUnixTimestamp(DateTime dateTime)
        {
            try
            {
                return (Int64)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            }
            catch
            {
                return 0;
            }
        }

        public static DateTime FromUnixTimestamp(Int64 unixTimestamp)
        {
            try
            {
                return (new DateTime(1970, 1, 1)).AddSeconds(unixTimestamp);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
