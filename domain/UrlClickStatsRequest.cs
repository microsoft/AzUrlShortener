namespace Cloud5mins.ShortenerTools.Domain
{
    public class UrlClickStatsRequest
    {
        public string Vanity { get; set; }

        public UrlClickStatsRequest(string vanity)
        {
            Vanity = vanity;
        }
    }
}