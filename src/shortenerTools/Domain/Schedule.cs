using System;
using Cronos;

namespace Cloud5mins.domain
{
    public class Schedule
    {
        public DateTime Start { get; set; } = DateTime.MinValue;
        public DateTime End { get; set; } = DateTime.MaxValue;

        public string AlternativeUrl { get; set; } = "";
        public string Cron { get; set; } = "0 0 0 0 0";

        public int DurationMinutes { get; set; } = 0;

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
