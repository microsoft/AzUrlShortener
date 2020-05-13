using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace Cloud5mins.domain
{
    public class ShortUrlEntity : TableEntity
    {
        //public string Id { get; set; }
        public string Url { get; set; }

        public string Title { get; set; }

        public string ShortUrl { get; set; }

        public int Clicks { get; set; }

        public ShortUrlEntity(){}

        public ShortUrlEntity(string longUrl, string endUrl){
            initialize(longUrl, endUrl, string.Empty);
        }

        public ShortUrlEntity(string longUrl, string endUrl, string title){
            initialize(longUrl, endUrl, title);
        }

        private void initialize(string longUrl, string endUrl, string title){
            PartitionKey = endUrl.First().ToString();
            RowKey = endUrl;
            Url = longUrl;
            Title = title;
            Clicks = 0;
        }

        public static ShortUrlEntity GetEntity(string longUrl, string endUrl){
            return new ShortUrlEntity
            {
                PartitionKey = endUrl.First().ToString(),
                RowKey = endUrl,
                Url = longUrl
            };
        }
    }


}
