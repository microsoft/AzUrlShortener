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

	public UrlServices(ShortenerSettings settings, ILogger logger)
	{
		_settings = settings;
		_logger = logger;
	}

	public async Task<string> Redirect(string shortUrl)
	{
		string redirectUrl = "https://azure.com";

		if (!string.IsNullOrWhiteSpace(shortUrl))
		{
			redirectUrl = _settings.DefaultRedirectUrl ?? redirectUrl;

			StorageTableHelper stgHelper = new StorageTableHelper(_settings.DataStorage);

			var tempUrl = new ShortUrlEntity(string.Empty, shortUrl);
			var newUrl = await stgHelper.GetShortUrlEntity(tempUrl);

			if (newUrl != null)
			{
				_logger.LogInformation($"Found it: {newUrl.Url}");
				newUrl.Clicks++;
				await stgHelper.SaveClickStatsEntity(new ClickStatsEntity(newUrl.RowKey));
				await stgHelper.SaveShortUrlEntity(newUrl);
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

            StorageTableHelper stgHelper = new StorageTableHelper(_settings.DataStorage);

            try
            {
                result.UrlList = await stgHelper.GetAllShortUrlEntities();
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
}