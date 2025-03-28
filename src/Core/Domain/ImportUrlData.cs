namespace Cloud5mins.ShortenerTools.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UrlDetails
    {
        public required NextId NextId { get; set; }
        public required List<ShortUrlEntity> LstShortUrlEntity { get; set; }
    }
}
