using Cloud5mins.domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using shortenerTools.Abstractions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

namespace Cloud5mins.Function
{
    public class UrlRedirect
    {
        private readonly IUserIpLocationService _userIpLocationService;
        private readonly IConfiguration _configuration;

        public UrlRedirect(IUserIpLocationService userIpLocationService, IConfiguration configuration)
        {
            _userIpLocationService = userIpLocationService;
            _configuration = configuration;
        }

        [FunctionName("UrlRedirect")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "UrlRedirect/{shortUrl}")] HttpRequestMessage req,
            string shortUrl,
            ExecutionContext context,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed for Url: {shortUrl}");

            var redirectUrl = "https://azure.com";

            if (!string.IsNullOrWhiteSpace(shortUrl))
            {
                redirectUrl = _configuration["defaultRedirectUrl"];

                var stgHelper = new StorageTableHelper(_configuration["UlsDataStorage"]);

                var tempUrl = new ShortUrlEntity(string.Empty, shortUrl);

                var newUrl = await stgHelper.GetShortUrlEntity(tempUrl);

                if (newUrl != null)
                {
                    log.LogInformation($"Found it: {newUrl.Url}");
                    await SetUrlClickStatsAsync(newUrl);
                    stgHelper.SaveClickStatsEntity(new ClickStatsEntity(newUrl.RowKey));
                    await stgHelper.SaveShortUrlEntity(newUrl);
                    redirectUrl = WebUtility.UrlDecode(newUrl.Url);
                }
            }
            else
            {
                log.LogInformation("Bad Link, resorting to fallback.");
            }

            var res = req.CreateResponse(HttpStatusCode.Redirect);
            res.Headers.Add("Location", redirectUrl);
            return res;
        }

        private async Task SetUrlClickStatsAsync(ShortUrlEntity newUrl)
        {
            var userIpResponse = await _userIpLocationService.GetUserIpAsync(CancellationToken.None);
            if (newUrl.Clicks.ContainsKey(userIpResponse.CountryName))
            {
                newUrl.Clicks[userIpResponse.CountryName] = newUrl.Clicks[userIpResponse.CountryName]++;
            }
            else
            {
                newUrl.Clicks.Add(userIpResponse.CountryName, 1);
            }
        }
    }
}
