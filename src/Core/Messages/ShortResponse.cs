namespace Cloud5mins.ShortenerTools.Core.Messages
{
    public class ShortResponse
    {
        public string ShortUrl { get; set; }
        public string LongUrl { get; set; }
        public string Title { get; set; }
        public string QrCode { get; set; }

        public ShortResponse() { }
        public ShortResponse(string host, string longUrl, string endUrl, string title)
        {
            LongUrl = longUrl;
            ShortUrl = string.Concat(host, "/", endUrl);
            Title = title;
        }

        public ShortResponse(string host, string longUrl, string endUrl, string title, string qrCode)
        {
            LongUrl = longUrl;
            ShortUrl = string.Concat(host, "/", endUrl);
            Title = title;
            QrCode = qrCode;
        }
    }
}