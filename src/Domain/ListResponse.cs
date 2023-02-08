using System.Collections.Generic;

namespace Cloud5mins.ShortenerTools.Domain
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