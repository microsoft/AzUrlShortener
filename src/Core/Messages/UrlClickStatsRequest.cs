namespace Cloud5mins.ShortenerTools.Core.Messages
{
    public class UrlClickStatsRequest
    {
        public string Vanity { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public UrlClickStatsRequest(string vanity, string startDate, string endDate)
        {
            Vanity = vanity;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}