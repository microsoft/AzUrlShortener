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
}
