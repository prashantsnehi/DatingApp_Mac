using System.Globalization;
using System;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static Int32 DateTimeToUnixTimeStamp(this DateTime datetime) => (Int32)(datetime.Subtract(new DateTime(1970,1,1)).TotalSeconds);
        public static DateTime UnixTimeStampToDateTime(this int unixTimeStamp) => new DateTime(1970,1,1).AddSeconds(unixTimeStamp);
        public static int CalculateAge(this DateTime dob) {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if(dob.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}