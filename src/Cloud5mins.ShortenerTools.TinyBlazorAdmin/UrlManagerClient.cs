using System;
using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Messages;

namespace Cloud5mins.ShortenerTools.TinyBlazorAdmin;

public class UrlManagerClient(HttpClient httpClient)
{

	public async Task<IQueryable<ShortUrlEntity>?> GetUrls()
    {
		IQueryable<ShortUrlEntity> urlList = null;
		try{
			using var response = await httpClient.GetAsync("/api/UrlList");
			if(response.IsSuccessStatusCode){
				var urls = await response.Content.ReadFromJsonAsync<ListResponse>();
				urlList = urls!.UrlList.AsQueryable<ShortUrlEntity>();
			}
		}
		catch(Exception ex){
			Console.WriteLine(ex.Message);
		}
        
		return urlList;
    }

	public async Task<(bool , string)> UrlCreate(ShortRequest url)
	{
		(bool , string) result = (false, "Failed");
		try{
			using var response = await httpClient.PostAsJsonAsync<ShortRequest>("/api/UrlCreate", url);
			if(response.IsSuccessStatusCode){
				result = (true, "Success");
			}
			else{
				var errorDetails = await response.Content.ReadFromJsonAsync<DetailedBadRequest>();
				result = (false, errorDetails!.Message);
			}
		}
		catch(Exception ex){
			Console.WriteLine(ex.Message);
			result = (false, ex.Message);
		}
		return result;
	}

	public async Task<bool> UrlArchive(ShortUrlEntity shortUrl)
	{
		try{
			using var response = await httpClient.PostAsJsonAsync("/api/UrlArchive", shortUrl);
			if(response.IsSuccessStatusCode){
				return true;
			}
		}
		catch(Exception ex){
			Console.WriteLine(ex.Message);
		}
		
		return false;
	}

	public async Task<ShortUrlEntity?> UrlUpdate(ShortUrlEntity shortUrl)
	{
		try
		{
			using var response = await httpClient.PostAsJsonAsync("/api/UrlUpdate", shortUrl);
			if (response.IsSuccessStatusCode)
			{
				var updatedUrl = await response.Content.ReadFromJsonAsync<ShortUrlEntity>();
				return updatedUrl;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}

		return null;
	}

	public async Task<ClickDateList?> UrlClickStatsByDay(UrlClickStatsRequest statsRequest)
	{
		try
		{
			using var response = await httpClient.PostAsJsonAsync("/api/UrlClickStatsByDay", statsRequest);
			if (response.IsSuccessStatusCode)
			{
				var clickStats = await response.Content.ReadFromJsonAsync<ClickDateList>();
				return clickStats;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}

		return null;
	}
}
