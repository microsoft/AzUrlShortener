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
            TableOperation selOperation = TableOperation.Retrieve<NextId>("1", "Key");
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