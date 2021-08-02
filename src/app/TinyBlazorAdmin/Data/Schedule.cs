using System;

namespace TinyBlazorAdmin.Data
{
public class Schedule
    {
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now;

        public string AlternativeUrl { get; set; } = "";
        public string Cron { get; set; } = "0 0 0 0 0";

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
    }
}
