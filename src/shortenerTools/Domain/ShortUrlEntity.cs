using Microsoft.Azure.Cosmos.Table;
using shortenerTools;
using System.Collections.Generic;
using System.Linq;

namespace Cloud5mins.domain
{
    public class ShortUrlEntity : TableEntity
    {
        //public string Id { get; set; }
        public string Url { get; set; }

        public string Title { get; set; }

        public string ShortUrl { get; set; }

        [EntityJsonPropertyConverter]
        public Dictionary<string, int> Clicks { get; set; }

        public bool? IsArchived { get; set; }

        public ShortUrlEntity() { }

        public ShortUrlEntity(string longUrl, string endUrl)
        {
            Initialize(longUrl, endUrl, string.Empty);
        }

        public ShortUrlEntity(string longUrl, string endUrl, string title)
        {
            Initialize(longUrl, endUrl, title);
        }

        private void Initialize(string longUrl, string endUrl, string title)
        {
            PartitionKey = endUrl.First().ToString();
            RowKey = endUrl;
            Url = longUrl;
            Title = title;
            Clicks = new Dictionary<string, int>();
            IsArchived = false;
        }

        public static ShortUrlEntity GetEntity(string longUrl, string endUrl, string title)
        {
            return new ShortUrlEntity
            {
                PartitionKey = endUrl.First().ToString(),
                RowKey = endUrl,
                Url = longUrl,
                Title = title
            };
        }
    }


}
