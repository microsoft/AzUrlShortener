using System.Collections.Generic;
using Cloud5mins.ShortenerTools.Core.Domain;

namespace Cloud5mins.ShortenerTools.Core.Messages
{
    public class ListResponse
    {
        public List<ShortUrlEntity> UrlList { get; set; }

        public ListResponse() { }
        public ListResponse(List<ShortUrlEntity> list)
        {
            UrlList = list;
        }
    }
}