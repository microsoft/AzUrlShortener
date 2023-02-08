using Microsoft.Azure.Cosmos.Table;

namespace Cloud5mins.ShortenerTools.Domain
{
    public class NextId : TableEntity
    {
        public int Id { get; set; }
    }
}