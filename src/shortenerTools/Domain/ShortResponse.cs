using System;

namespace Cloud5mins.domain
{
    public class ShortResponse
    {
        public string ShortUrl { get; set; }
        public string LongUrl { get; set; }

        public ShortResponse(){}
        public ShortResponse (string host, string longUrl, string endUrl)
        {
            LongUrl = longUrl;
            ShortUrl = string.Concat(host, "/", endUrl);
        }
    }
}