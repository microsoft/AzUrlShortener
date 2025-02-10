using System.Collections.Generic;
using System.Threading.Tasks;
using Cloud5mins.ShortenerTools.Core.Domain;

namespace Cloud5mins.ShortenerTools.Core.Service;

public interface IAzStrorageTablesService
{
    Task<List<ShortUrlEntity2>> GetAllShortUrlEntities();
    Task<ShortUrlEntity2> SaveShortUrlEntity(ShortUrlEntity2 newRow2);

}
