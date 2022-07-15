using System;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Cloud5mins.Functions
{
    public class UrlRedirect
    {
        private readonly ILogger _logger;

        public UrlRedirect(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
        }

        [Function("UrlRedirect")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "UrlRedirect/{shortUrl}")] HttpRequestData req,
            string shortUrl, 
            ExecutionContext context)
        {
            _logger.LogInformation($"-->> TRying to Url Redirect");
            _logger.LogInformation($"HTTP trigger function processed for Url: {shortUrl}");

            string redirectUrl = "https://azure.com";

            if (!String.IsNullOrWhiteSpace(shortUrl))
            {
                // var config = new ConfigurationBuilder()
                //     .SetBasePath(context.FunctionAppDirectory)
                //     .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
                //     .AddEnvironmentVariables()
                //     .Build();

                // redirectUrl = config["defaultRedirectUrl"];

                // StorageTableHelper stgHelper = new StorageTableHelper(config["UlsDataStorage"]); 

                // var tempUrl = new ShortUrlEntity(string.Empty, shortUrl);
                
                // var newUrl = await stgHelper.GetShortUrlEntity(tempUrl);

                var newUrl = "https://frankysnotes.com";

                if (newUrl != null)
                {
                    //log.LogInformation($"Found it: {newUrl.Url}");
                    //newUrl.Clicks++;
                    // stgHelper.SaveClickStatsEntity(new ClickStatsEntity(newUrl.RowKey));
                    // await stgHelper.SaveShortUrlEntity(newUrl);
                    //redirectUrl = WebUtility.UrlDecode(newUrl.ActiveUrl);
                    redirectUrl = WebUtility.UrlDecode(newUrl);
                }
            }
            else
            {
                _logger.LogInformation("Bad Link, resorting to fallback.");
            }

            var res = req.CreateResponse(HttpStatusCode.Redirect);
            res.Headers.Add("Location", redirectUrl);
            return res;
        }

        private string GetSummary(int temp)
        {
            var summary = "Mild";

            if (temp >= 32)
            {
                summary = "Hot";
            }
            else if (temp <= 16 && temp > 0)
            {
                summary = "Cold";
            }
            else if (temp <= 0)
            {
                summary = "Freezing";
            }

            return summary;
        }
    }
}
