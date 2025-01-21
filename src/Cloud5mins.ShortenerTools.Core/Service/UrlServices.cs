using Cloud5mins.ShortenerTools;
using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Messages;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Cloud5mins.ShortenerTools.Core.Services;

public class UrlServices
{
	private readonly ShortenerSettings _settings;
	private readonly ILogger _logger;
	private readonly StorageTableHelper _stgHelper;

	public UrlServices(ShortenerSettings settings, ILogger logger)
	{
		_settings = settings;
		_logger = logger;
	}

	private StorageTableHelper StgHelper => _stgHelper ?? new StorageTableHelper(_settings.DataStorage);

	public async Task<ShortUrlEntity> Archive(ShortUrlEntity input)
	{
		ShortUrlEntity result = await _stgHelper.ArchiveShortUrlEntity(input);
		return result;
	}
	public async Task<string> Redirect(string shortUrl)
	{
		string redirectUrl = "https://azure.com";
		try
		{
			if (!string.IsNullOrWhiteSpace(shortUrl))
			{
				redirectUrl = _settings.DefaultRedirectUrl ?? redirectUrl;

				var tempUrl = new ShortUrlEntity(string.Empty, shortUrl);
				var newUrl = await StgHelper.GetShortUrlEntity(tempUrl);

				if (newUrl != null)
				{
					_logger.LogInformation($"Found it: {newUrl.Url}");
					newUrl.Clicks++;
					await StgHelper.SaveClickStatsEntity(new ClickStatsEntity(newUrl.RowKey));
					await StgHelper.SaveShortUrlEntity(newUrl);
					redirectUrl = WebUtility.UrlDecode(newUrl.ActiveUrl);
				}
			}
			else
			{
				_logger.LogInformation("Bad Link, resorting to fallback.");
			}
		}
		catch (Exception ex)
		{
			_logger.LogInformation($"Problem accessing storage: {ex.Message}");
		}
		return redirectUrl;
	}

	public async Task<ListResponse> List(string host)
	{
		_logger.LogInformation($"Starting UrlList...");

		var result = new ListResponse();
		string userId = string.Empty;

		try
		{
			result.UrlList = await StgHelper.GetAllShortUrlEntities();
			result.UrlList = result.UrlList.Where(p => !(p.IsArchived ?? false)).ToList();
			foreach (ShortUrlEntity url in result.UrlList)
			{
				url.ShortUrl = Utility.GetShortUrl(host, url.RowKey);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An unexpected error was encountered.");
			throw;
		}

		return result;
	}

	public async Task<ShortResponse> Create(ShortRequest input, string host)
	{
		ShortResponse result;

		try
		{

			// If the Url parameter only contains whitespaces or is empty return with BadRequest.
			if (string.IsNullOrWhiteSpace(input.Url))
			{
				throw new ShortenerToolException(HttpStatusCode.BadRequest, "The url parameter can not be empty.");
			}

			// Validates if input.url is a valid aboslute url, aka is a complete refrence to the resource, ex: http(s)://google.com
			if (!Uri.IsWellFormedUriString(input.Url, UriKind.Absolute))
			{
				throw new ShortenerToolException(HttpStatusCode.BadRequest, $"{input.Url} is not a valid absolute Url. The Url parameter must start with 'http://' or 'http://'.");
			}

			string longUrl = input.Url.Trim();
			string vanity = string.IsNullOrWhiteSpace(input.Vanity) ? "" : input.Vanity.Trim();
			string title = string.IsNullOrWhiteSpace(input.Title) ? "" : input.Title.Trim();

			ShortUrlEntity newRow;

			if (!string.IsNullOrEmpty(vanity))
			{
				newRow = new ShortUrlEntity(longUrl, vanity, title, input.Schedules);

				if (await StgHelper.IfShortUrlEntityExist(newRow))
				{
					throw new ShortenerToolException(HttpStatusCode.Conflict, "This Short URL already exist.");
				}
			}
			else
			{
				newRow = new ShortUrlEntity(longUrl, await Utility.GetValidEndUrl(vanity, StgHelper), title, input.Schedules);
			}

			await StgHelper.SaveShortUrlEntity(newRow);

			result = new ShortResponse(host, newRow.Url, newRow.RowKey, newRow.Title);

			_logger.LogInformation("Short Url created.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An unexpected error was encountered.");
			throw;
		}

		return result;
	}

	public async Task<ShortUrlEntity> Update(ShortUrlEntity input, string host)
	{
		ShortUrlEntity result;

		try
		{
			// If the Url parameter only contains whitespaces or is empty return with BadRequest.
			if (string.IsNullOrWhiteSpace(input.Url))
			{
				throw new ShortenerToolException(HttpStatusCode.BadRequest, "The url parameter can not be empty.");
			}

			// Validates if input.url is a valid aboslute url, aka is a complete refrence to the resource, ex: http(s)://google.com
			if (!Uri.IsWellFormedUriString(input.Url, UriKind.Absolute))
			{
				throw new ShortenerToolException(HttpStatusCode.BadRequest, $"{input.Url} is not a valid absolute Url. The Url parameter must start with 'http://' or 'http://'.");
			}

			result = await StgHelper.UpdateShortUrlEntity(input);
			result.ShortUrl = Utility.GetShortUrl(host, result.RowKey);

		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An unexpected error was encountered.");
			throw;
		}

		return result;
	}


	public async Task<ClickDateList> ClickStatsByDay(UrlClickStatsRequest input, string host)
	{
		var result = new ClickDateList();
		try
		{
			var rawStats = await StgHelper.GetAllStatsByVanity(input.Vanity);

			result.Items = rawStats.GroupBy(s => DateTime.Parse(s.Datetime).Date)
										.Select(stat => new ClickDate
										{
											DateClicked = stat.Key.ToString("yyyy-MM-dd"),
											Count = stat.Count()
										}).OrderBy(s => DateTime.Parse(s.DateClicked).Date).ToList<ClickDate>();

			result.Url = Utility.GetShortUrl(host, input.Vanity);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An unexpected error was encountered.");
			throw;
		}
		return result;
	}
}

