using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Messages;
using Cloud5mins.ShortenerTools.Core.Service;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Cloud5mins.ShortenerTools.Core.Services;

public class UrlServices
{
    private readonly ILogger _logger;
    private readonly IAzStrorageTablesService _stgHelper;

    public UrlServices(ILogger logger, IAzStrorageTablesService stgHelper)
    {
        _logger = logger;
        _stgHelper = stgHelper;
    }

    public async Task<ShortUrlEntity> Archive(ShortUrlEntity input)
    {
        ShortUrlEntity result = await _stgHelper.ArchiveShortUrlEntity(input);
        return result;
    }
    public async Task<string> Redirect(string? shortUrl)
    {
        string redirectUrl = "https://azure.com";
        try
        {
            redirectUrl = Environment.GetEnvironmentVariable("DefaultRedirectUrl") ?? redirectUrl;

            if (!string.IsNullOrWhiteSpace(shortUrl))
            {
                var tempUrl = new ShortUrlEntity(string.Empty, shortUrl);
                var newUrl = await _stgHelper.GetShortUrlEntity(tempUrl);

                if (newUrl != null)
                {
                    _logger.LogInformation($"Found it: {newUrl.Url}");
                    newUrl.Clicks++;
                    await _stgHelper.SaveClickStatsEntity(new ClickStatsEntity(newUrl.RowKey));
                    await _stgHelper.SaveShortUrlEntity(newUrl);
                    redirectUrl = WebUtility.UrlDecode(newUrl.ActiveUrl);
                }
                else
                {
                    _logger.LogInformation("Bad Link, resorting to fallback.");
                }
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
            result.UrlList = await _stgHelper.GetAllShortUrlEntities();
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

                if (await _stgHelper.IfShortUrlEntityExist(newRow))
                {
                    throw new ShortenerToolException(HttpStatusCode.Conflict, "This Short URL already exist.");
                }
            }
            else
            {
                var generatedVanity = await Utility.GetValidEndUrl(vanity, _stgHelper);
                newRow = new ShortUrlEntity(longUrl, generatedVanity, title, input.Schedules);
            }

            await _stgHelper.SaveShortUrlEntity(newRow);

            result = new ShortResponse(host, newRow.Url, newRow.RowKey, newRow.Title);

            _logger.LogInformation("Short Url created.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
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

            result = await _stgHelper.UpdateShortUrlEntity(input);
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
            var rawStats = await _stgHelper.GetAllStatsByVanity(input.Vanity);

            result.Items = rawStats.GroupBy(s => DateTime.Parse(s.Datetime).Date)
                                        .Select(stat => new ClickDate
                                        {
                                            DateClicked = DateTime.Parse(stat.Key.ToString("yyyy-MM-dd")),
                                            Count = stat.Count()
                                        }).OrderBy(s => s.DateClicked).ToList<ClickDate>();

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

