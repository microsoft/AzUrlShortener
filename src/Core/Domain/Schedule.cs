using Cronos;
using System;

namespace Cloud5mins.ShortenerTools.Core.Domain
{
    public class Schedule
    {
        public DateTime Start { get; set; } = DateTime.Now.AddMonths(-6);
        public DateTime End { get; set; } = DateTime.Now.AddMonths(6);

        public string AlternativeUrl { get; set; } = "";
        public string Cron { get; set; } = "* * * * *";

        public int DurationMinutes { get; set; } = 0;

        public string GetDisplayableUrl(int max)
        {
            var length = AlternativeUrl.ToString().Length;
            if (length >= max)
            {
                return string.Concat(AlternativeUrl.Substring(0, max-1), "...");
            }
            return AlternativeUrl;
        }

        public bool IsActive(DateTime pointInTime)
        {
            var bufferStart = pointInTime.AddMinutes(-DurationMinutes);
            var expires = pointInTime.AddMinutes(DurationMinutes);

            CronExpression expression = CronExpression.Parse(Cron);
            var occurences = expression.GetOccurrences(bufferStart, expires);

            foreach (DateTime d in occurences)
            {
                if (d < pointInTime && d < expires)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
