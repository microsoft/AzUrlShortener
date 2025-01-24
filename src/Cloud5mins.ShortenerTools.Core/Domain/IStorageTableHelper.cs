using Microsoft.Azure.Cosmos.Table;
using System.Text.Json;

namespace Cloud5mins.ShortenerTools.Core.Domain;

public interface IStorageTableHelper
{
	CloudStorageAccount CreateStorageAccountFromConnectionString();
	Task<ShortUrlEntity> GetShortUrlEntity(ShortUrlEntity row);
	Task<List<ShortUrlEntity>> GetAllShortUrlEntities();
	Task<ShortUrlEntity> GetShortUrlEntityByVanity(string vanity);
	Task SaveClickStatsEntity(ClickStatsEntity newStats);
	Task<ShortUrlEntity> SaveShortUrlEntity(ShortUrlEntity newShortUrl);
	Task<bool> IfShortUrlEntityExistByVanity(string vanity);
	Task<bool> IfShortUrlEntityExist(ShortUrlEntity row);
	Task<int> GetNextTableId();
	Task<ShortUrlEntity> UpdateShortUrlEntity(ShortUrlEntity urlEntity);
	Task<List<ClickStatsEntity>> GetAllStatsByVanity(string vanity);
	Task<ShortUrlEntity> ArchiveShortUrlEntity(ShortUrlEntity urlEntity);
}
