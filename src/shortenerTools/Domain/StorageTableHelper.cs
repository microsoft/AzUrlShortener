using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Cloud5mins.domain
{

    public class StorageTableHelper
    {
        private string StorageConnectionString { get; set; }

        public StorageTableHelper(){}

        public StorageTableHelper(string storageConnectionString){
            StorageConnectionString = storageConnectionString;
        }

       public CloudStorageAccount CreateStorageAccountFromConnectionString()
       {
           CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.StorageConnectionString);
           return storageAccount;
       }

        private  CloudTable GetUrlsTable(){
            CloudStorageAccount storageAccount = this.CreateStorageAccountFromConnectionString();
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference("UrlsDetails");
            table.CreateIfNotExists();

            return table;
        }

        public async Task<ShortUrlEntity> GetShortUrlEntity(ShortUrlEntity row)
        {
             TableOperation selOperation = TableOperation.Retrieve<ShortUrlEntity>(row.PartitionKey, row.RowKey);
             TableResult result = await GetUrlsTable().ExecuteAsync(selOperation);
             ShortUrlEntity eShortUrl = result.Result as ShortUrlEntity;
             return eShortUrl;
        }

        public async Task<List<ShortUrlEntity>> GetAllShortUrlEntities()
        {
            var tblUrls = GetUrlsTable();
            TableContinuationToken token = null;
            var lstShortUrl = new List<ShortUrlEntity>();
            do
            {
                // Retreiving all entities that are NOT the NextId entity 
                // (it's the only one in the partion "KEY")
                TableQuery<ShortUrlEntity> rangeQuery = new TableQuery<ShortUrlEntity>().Where(
                    filter: TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.NotEqual, "KEY"));

                var queryResult = await tblUrls.ExecuteQuerySegmentedAsync(rangeQuery, token);
                lstShortUrl.AddRange(queryResult.Results as List<ShortUrlEntity>);
                token = queryResult.ContinuationToken;
            } while (token != null);
            return lstShortUrl;
        }

        public  async Task<bool> IfShortUrlEntityExist(ShortUrlEntity row)
        {
             ShortUrlEntity eShortUrl = await GetShortUrlEntity(row);
             return (eShortUrl != null);
        }

        public  async Task<ShortUrlEntity> SaveShortUrlEntity(ShortUrlEntity newShortUrl)
        {
             TableOperation insOperation = TableOperation.InsertOrMerge(newShortUrl);
             TableResult result = await GetUrlsTable().ExecuteAsync(insOperation);
             ShortUrlEntity eShortUrl = result.Result as ShortUrlEntity;
             return eShortUrl;
        }  

        public  async Task<int> GetNextTableId()
        {
            //Get current ID
            TableOperation selOperation = TableOperation.Retrieve<NextId>("1", "KEY");
            TableResult result = await GetUrlsTable().ExecuteAsync(selOperation);
            NextId entity = result.Result as NextId;

            if(entity == null){
                entity = new NextId{
                    PartitionKey = "1",
                    RowKey = "KEY",
                    Id = 1024
                };                   
            }
            entity.Id++;

            //Update
            TableOperation updOperation = TableOperation.InsertOrMerge(entity);

            // Execute the operation.
            await GetUrlsTable().ExecuteAsync(updOperation);

            return entity.Id;
        }
    }
}