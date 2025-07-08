namespace Cloud5mins.ShortenerTools.BatchShortener.Models
{
    public class CosmosDBEntry
    {
        public string id { get; set; } = string.Empty;
        public string? Article_GUID { get; set; }
        public string? Article_Title { get; set; }
        public string? Share_Url { get; set; }
    }
}
