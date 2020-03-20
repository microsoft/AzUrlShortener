using System;
// using System.IO;
using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
// using Microsoft.AspNetCore.Http;
// using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
// using Newtonsoft.Json;
// using System.Linq;
using System.Net;
using System.Net.Http;
using Cloud5mins.domain;
//using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Extensions.Configuration;
// using Microsoft.Azure.Cosmos.Table;

namespace Cloud5mins.Function
{
    public static class UrlRedirect
    {
        [FunctionName("UrlRedirect")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "UrlRedirect/{shortUrl}")] HttpRequestMessage req,
            string shortUrl, 
            ExecutionContext context,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed for Url: {shortUrl}");

            var redirectUrl = "http://azure.com";

            if (!String.IsNullOrWhiteSpace(shortUrl))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                StorageTableHelper stgHelper = new StorageTableHelper(config["UlsDataStorage"]); 

                var tempUrl = new ShortUrlEntity(string.Empty, shortUrl);
                
                var newUrl = await stgHelper.GetShortUrlEntity(tempUrl);

                if (newUrl != null)
                {
                    log.LogInformation($"Found it: {newUrl.Url}");
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
  }
}
