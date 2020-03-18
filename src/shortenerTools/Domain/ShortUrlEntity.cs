using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace Cloud5mins.domain
{
    public class ShortUrlEntity : TableEntity
    {
        //public string Id { get; set; }
        public string Url { get; set; }

        public ShortUrlEntity(){}

        public ShortUrlEntity(string longUrl, string endUrl){
            PartitionKey = endUrl.First().ToString();
            RowKey = endUrl;
            Url = longUrl;
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
