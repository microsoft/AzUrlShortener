using Cloud5mins.ShortenerTools.Core.Domain;

namespace Cloud5mins.ShortenerTools.Core.Service;

public interface IAzStrorageTablesService
{
    Task<int> GetNextTableId();
    Task<List<ShortUrlEntity>> GetAllShortUrlEntities();
    Task<ShortUrlEntity> SaveShortUrlEntity(ShortUrlEntity newRow2);
    Task<ShortUrlEntity> GetShortUrlEntity(ShortUrlEntity row);
    Task<bool> IfShortUrlEntityExist(ShortUrlEntity row);
    Task<ShortUrlEntity> UpdateShortUrlEntity(ShortUrlEntity urlEntity);
    Task<ShortUrlEntity?> GetShortUrlEntityByVanity(string vanity);
    Task<bool> IfShortUrlEntityExistByVanity(string vanity);
    Task<ShortUrlEntity> ArchiveShortUrlEntity(ShortUrlEntity urlEntity);
    Task<List<ClickStatsEntity>> GetAllStatsByVanity(string vanity);
    Task SaveClickStatsEntity(ClickStatsEntity newStats);
}
