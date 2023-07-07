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

	public async Task<string> Redirect(string shortUrl)
	{
		string redirectUrl = "https://azure.com";

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
			throw ex;
		}

		return result;
	}

	public async Task<ShortResponse> Create(ShortRequest input, string host)
	{
			_logger.LogInformation($"__trace creating shortURL: {input.Url}");
            string userId = string.Empty;
            var result = new ShortResponse();

			try
            {

                // If the Url parameter only contains whitespaces or is empty return with BadRequest.
                if (string.IsNullOrWhiteSpace(input.Url))
                {
					var shortEx = new ShortenerToolException("The url parameter can not be empty.");
					shortEx.StatusCode = HttpStatusCode.BadRequest;
					throw shortEx;
                }

                // Validates if input.url is a valid aboslute url, aka is a complete refrence to the resource, ex: http(s)://google.com
                if (!Uri.IsWellFormedUriString(input.Url, UriKind.Absolute))
                {
					var shortEx = new ShortenerToolException($"{input.Url} is not a valid absolute Url. The Url parameter must start with 'http://' or 'http://'.");
					shortEx.StatusCode = HttpStatusCode.BadRequest;
					throw shortEx;
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
						var shortEx = new ShortenerToolException("This Short URL already exist.");
						shortEx.StatusCode = HttpStatusCode.Conflict;
						throw shortEx;
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

}

