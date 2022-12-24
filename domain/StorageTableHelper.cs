using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Cloud5mins.domain
{
	class StorageTableHelper
	{
		private string StorageConnectionString { get; set; }

		public StorageTableHelper() { }

		public StorageTableHelper(string storageConnectionString)
		{
			StorageConnectionString = storageConnectionString;
		}
		public CloudStorageAccount CreateStorageAccountFromConnectionString()
		{
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.StorageConnectionString);
			return storageAccount;
		}

		private CloudTable GetTable(string tableName)
		{
			CloudStorageAccount storageAccount = this.CreateStorageAccountFromConnectionString();
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
			CloudTable table = tableClient.GetTableReference(tableName);
			table.CreateIfNotExists();

			return table;
		}
		private CloudTable GetUrlsTable()
		{
			CloudTable table = GetTable("UrlsDetails");
			return table;
		}

		private CloudTable GetStatsTable()
		{
			CloudTable table = GetTable("ClickStats");
			return table;
		}

		public async Task<ShortUrlEntity> GetShortUrlEntity(ShortUrlEntity row)
		{
			TableOperation selOperation = TableOperation.Retrieve<ShortUrlEntity>(row.PartitionKey, row.RowKey);
			TableResult result = await GetUrlsTable().ExecuteAsync(selOperation);
			ShortUrlEntity eShortUrl = result.Result as ShortUrlEntity;
			return eShortUrl;
		}

		public async Task SaveClickStatsEntity(ClickStatsEntity newStats)
		{
			TableOperation insOperation = TableOperation.InsertOrMerge(newStats);
			TableResult result = await GetStatsTable().ExecuteAsync(insOperation);
		}

		public async Task<ShortUrlEntity> SaveShortUrlEntity(ShortUrlEntity newShortUrl)
		{
			TableOperation insOperation = TableOperation.InsertOrMerge(newShortUrl);
			TableResult result = await GetUrlsTable().ExecuteAsync(insOperation);
			ShortUrlEntity eShortUrl = result.Result as ShortUrlEntity;
			return eShortUrl;
		}

		public  void CreateTables()
		{
			GetTable("UrlsDetails");
			GetTable("UrlsDetails");
		}
	}
}