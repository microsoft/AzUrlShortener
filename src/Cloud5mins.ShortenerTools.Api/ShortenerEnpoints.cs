using System.Data;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;
using Azure.Data.Tables;
using Cloud5mins.ShortenerTools;
using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Messages;
using Cloud5mins.ShortenerTools.Core.Service;
using Microsoft.AspNetCore.Http.HttpResults;

public static class ShortenerEnpoints
{
	public static void MapShortenerEnpoints(this IEndpointRouteBuilder app)
	{
		var endpoints = app.MapGroup("api")
				.WithOpenApi();

		endpoints.MapGet("/", GetWelcomeMessage)
			.WithDescription("Welcome to Cloud5mins URL Shortener API");

		endpoints.MapPost("/UrlCreate", UrlCreate)
			.WithDescription("Create a new Short URL")
			.WithDisplayName("Url Create");

		endpoints.MapGet("/UrlList", UrlList)
			.WithDescription("List all Urls")
			.WithDisplayName("Url List");

	}

	static private string GetWelcomeMessage()
	{
		return "Welcome to Cloud5mins URL Shortener API";
	}

	static private async Task<Results<
								Created<ShortResponse>,
								BadRequest<DetailedBadRequest>,
								NotFound<DetailedBadRequest>,
								Conflict<DetailedBadRequest>,
								InternalServerError<DetailedBadRequest>
								>> UrlCreate(ShortRequest request,  
												TableServiceClient tblClient, 
												HttpContext context, 
												ILogger logger)
	{

		logger.LogTrace($"creating shortURL: {request.Url}");
		var result = new ShortResponse();
		IAzStrorageTablesService stgHelper = new AzStrorageTablesService(tblClient);

		try
		{
			// If the Url parameter only contains whitespaces or is empty return with BadRequest.
			if (string.IsNullOrWhiteSpace(request.Url))
			{
				string ErrorMsg = "The url parameter cannot be empty.";
				logger.LogInformation(ErrorMsg);
				return TypedResults.NotFound<DetailedBadRequest>(new DetailedBadRequest { Message = ErrorMsg });
			}

			// Validates if input.url is a valid aboslute url, aka is a complete refrence to the resource, ex: http(s)://google.com
			if (!Uri.IsWellFormedUriString(request.Url, UriKind.Absolute))
			{
				string ErrorMsg = $"{request.Url} is not a valid absolute Url. The Url parameter must start with 'http://' or 'http://'.";
				logger.LogInformation(ErrorMsg);
				return TypedResults.BadRequest<DetailedBadRequest>(new DetailedBadRequest { Message = ErrorMsg });
			}

			string longUrl = request.Url.Trim();
			string vanity = string.IsNullOrWhiteSpace(request.Vanity) ? "" : request.Vanity.Trim();
			string title = string.IsNullOrWhiteSpace(request.Title) ? "" : request.Title.Trim();

			ShortUrlEntity? newRow;

			// if (!string.IsNullOrEmpty(vanity))
			// {
			// 	newRow = new ShortUrlEntity(longUrl, vanity, title, request.Schedules);
			// 	if (await stgHelper.IfShortUrlEntityExist(newRow))
			// 	{
			// 		// throw new Exception("This Short URL already exist.");
			// 		string ErrorMsg = "This Short URL already exist.";
			// 		logger.LogInformation(ErrorMsg);
			// 		return TypedResults.Conflict<DetailedBadRequest>(new DetailedBadRequest { Message = ErrorMsg });
			// 	}
			// }
			// else
			// {
			// 	newRow = new ShortUrlEntity(longUrl, await Utility.GetValidEndUrl(vanity, stgHelper), title, request.Schedules);
			// }

			ShortUrlEntity2 newRow2 = new ShortUrlEntity2(longUrl, vanity, title, request.Schedules);

			await stgHelper.SaveShortUrlEntity(newRow2);

			var host = GetHost(context);
			result = new ShortResponse(host!, newRow2.Url, newRow2.RowKey, newRow2.Title);

			logger.LogTrace("Short Url created.");

			return TypedResults.Created($"/api/UrlCreate/{result.ShortUrl}", result);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"A unexpected error was encountered: {ex.Message}");
			logger.LogError(ex.Message);
			return TypedResults.InternalServerError<DetailedBadRequest>(new DetailedBadRequest { Message = ex.Message });
		}
	}


	static private async Task<Results<
								Ok<ListResponse>,
								InternalServerError<DetailedBadRequest>
								>> UrlList(		//IAzStrorageTablesService stgHelper, 
												TableServiceClient tblClient,
												HttpContext context,
												ILogger logger)
	{
		logger.LogTrace("Starting UrlList...");

		var result = new ListResponse();
        string userId = string.Empty;
		IAzStrorageTablesService stgHelper = new AzStrorageTablesService(tblClient);

		try
		{
			// result.UrlList = await stgHelper.GetAllShortUrlEntities();
			// result.UrlList = result.UrlList.Where(p => !(p.IsArchived ?? false)).ToList();
			

			List<ShortUrlEntity2> allUrls = await stgHelper.GetAllShortUrlEntities();
			var filtredUlrs = allUrls.Where(p => !(p.IsArchived ?? false)).ToList();

			// Insert into result.UrlList all filtredUlrs mapping all properties
			result.UrlList = filtredUlrs.Select(p => new ShortUrlEntity
			{
				PartitionKey = p.PartitionKey,
				RowKey = p.RowKey,
				Url = p.Url,
				Title = p.Title,
				Clicks = p.Clicks,
				IsArchived = p.IsArchived,
				Schedules = p.Schedules
			}).ToList();



			var host = GetHost(context);

			foreach (ShortUrlEntity url in result.UrlList)
			{
				url.ShortUrl = Utility.GetShortUrl(host, url.RowKey);
			}
			return TypedResults.Ok(result);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An unexpected error was encountered.");
			return TypedResults.InternalServerError<DetailedBadRequest>(new DetailedBadRequest { Message = ex.Message });
		}
	}

	private static string GetHost(HttpContext context)
	{
		string? customDomain = Environment.GetEnvironmentVariable("CustomDomain");
		var host = string.IsNullOrEmpty(customDomain) ? context.Request.Host.Value : customDomain;
		return host ?? string.Empty;
	}

}

