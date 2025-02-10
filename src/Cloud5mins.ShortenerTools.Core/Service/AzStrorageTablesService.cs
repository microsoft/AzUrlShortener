using System;
using Azure.Data.Tables;
using Cloud5mins.ShortenerTools.Core.Domain;

namespace Cloud5mins.ShortenerTools.Core.Service;

public class AzStrorageTablesService(TableServiceClient client): IAzStrorageTablesService
{

	private TableClient GetUrlsTable()
	{
		client.CreateTableIfNotExists("UrlsDetails");
		TableClient table = client.GetTableClient("UrlsDetails");
		return table;
	}

	public async Task<List<ShortUrlEntity2>> GetAllShortUrlEntities()
	{
		TableClient tblUrls = GetUrlsTable();
		var lstShortUrl = new List<ShortUrlEntity2>();

		// Retreiving all entities that are NOT the NextId entity 
		// (it's the only one in the partion "KEY")
		var queryResult = tblUrls.QueryAsync<ShortUrlEntity2>(e => e.RowKey != "KEY");

		await foreach (var emp in queryResult.AsPages())
		{
			foreach (var item in emp.Values)
			{
				lstShortUrl.Add(item);
			}
		}

		return lstShortUrl;
	}


	    public async Task<ShortUrlEntity2> SaveShortUrlEntity(ShortUrlEntity2 newShortUrl)
        {
           
            // serializing the collection easier on json shares
            //newShortUrl.SchedulesPropertyRaw = JsonSerializer.Serialize<List<Schedule>>(newShortUrl.Schedules);

			TableClient tblUrls = GetUrlsTable();
			var response = await tblUrls.AddEntityAsync<ShortUrlEntity2>(newShortUrl);

			var temp = response.Content;
			return newShortUrl;
			

            // TableOperation insOperation = TableOanything peration.InsertOrMerge(newShortUrl);
            // TableResult result = await GetUrlsTable().ExecuteAsync(insOperation);
            // ShortUrlEntity eShortUrl = result.Result as ShortUrlEntity;
            // return eShortUrl;
        }

}
