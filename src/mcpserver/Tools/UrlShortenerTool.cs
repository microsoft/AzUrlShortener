
using System.ComponentModel;
using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Messages;
using ModelContextProtocol.Server;


[McpServerToolType]
public static class UrlShortenerTool
{
    [McpServerTool, Description("Shortens the given URL.")]
    public static string ShortenUrl(string longUrl)
    {
        if (longUrl.Length >= 3)
        {
            var last3Chars = longUrl[^3..]; // Get the last 3 characters
            return $"https://c5m.ca/{last3Chars}";
        }
        else
        {
            return "https://c5m.ca/test";
        }
    }

    // [McpServerTool, Description("Provide a list of all short URLs.")]
    // public static IQueryable<ShortUrlEntity> ListUrl()
    // {
    //     IQueryable<ShortUrlEntity> urlList = null;
    //     var httpClient = new HttpClient();
    //     httpClient.BaseAddress = new Uri("https://localhost:7187");

    //     try{
	// 		using var response = httpClient.GetAsync("/api/UrlList").Result;
	// 		if(response.IsSuccessStatusCode){
	// 			var urls = response.Content.ReadFromJsonAsync<ListResponse>().Result;
	// 			urlList = urls!.UrlList.AsQueryable<ShortUrlEntity>();
	// 		}
	// 	}
	// 	catch(Exception ex){
	// 		Console.WriteLine(ex.Message);
	// 	}
    //     return urlList;
    // }
}



///// VERSION 2 /////
///
// using System.ComponentModel;
// using Cloud5mins.ShortenerTools;
// using Cloud5mins.ShortenerTools.Core.Domain;
// using Cloud5mins.ShortenerTools.Core.Messages;
// using ModelContextProtocol.Server;
// using Microsoft.Extensions.DependencyInjection;

// [McpServerToolType]
// public class UrlShortenerTool
// {
//     private readonly UrlManagerClient _urlManager;

//     public UrlShortenerTool(UrlManagerClient urlManager)
//     {
//         _urlManager = urlManager;
//     }

//     [McpServerTool, Description("Shortens the given URL.")]
//     public string ShortenUrl(string longUrl, string? vanity)
//     {
//         ShortRequest urlRequest = new ShortRequest()
//         {
//             Url = longUrl,
//             Vanity = vanity ?? string.Empty
//         };

//         var response = _urlManager.UrlCreate(urlRequest).Result;
//         if (response.Item1)
//         {
//             return "Short URL created successfully";
//         }
//         else
//         {
//             return $"Failed to create short URL: {response.Item2}";
//         }
//     }

//     [McpServerTool, Description("Provide a list of all short URLs.")]
//     public List<ShortUrlEntity> ListUrl()
//     {
//         var urlList = _urlManager.GetUrls().Result.ToList<ShortUrlEntity>();
//         return urlList;
//     }
// }