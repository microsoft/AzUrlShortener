using Cloud5mins.ShortenerTools.Core.Domain;
using Microsoft.Extensions.Logging;
using System.Net;

public class UrlActions
{

	public async Task<string> Redirect(string shortUrl, ShortenerSettings settings, ILogger logger)
	{
		string redirectUrl = "https://azure.com";

		if (!string.IsNullOrWhiteSpace(shortUrl))
		{
			redirectUrl = settings.DefaultRedirectUrl ?? redirectUrl;

			StorageTableHelper stgHelper = new StorageTableHelper(settings.DataStorage);

			var tempUrl = new ShortUrlEntity(string.Empty, shortUrl);
			var newUrl = await stgHelper.GetShortUrlEntity(tempUrl);

			if (newUrl != null)
			{
				logger.LogInformation($"Found it: {newUrl.Url}");
				newUrl.Clicks++;
				await stgHelper.SaveClickStatsEntity(new ClickStatsEntity(newUrl.RowKey));
				await stgHelper.SaveShortUrlEntity(newUrl);
				redirectUrl = WebUtility.UrlDecode(newUrl.ActiveUrl);
			}
		}
		else
		{
			logger.LogInformation("Bad Link, resorting to fallback.");
		}

		return redirectUrl;
	}

}