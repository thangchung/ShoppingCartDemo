using System;

namespace NT.Infrastructure
{
    public static class DateTimeExtensions
    {
        public static DateTime GetCurrentUtcDateTime(this DateTime datetime, double timeZone = +7.0)
        {
            var utcOffset = TimeSpan.FromHours(timeZone);
            var result = new DateTimeOffset(datetime, utcOffset);
            return result.UtcDateTime;
        }
    }
}