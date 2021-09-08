using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Cloud5mins.domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace Cloud5mins.Function
{
    public static class UrlRedirect
    {
        //[OpenApiOperation(operationId: "UrlRedirect", tags: new[] { "Urls" }, Summary = "Redirect short url to long url", Description = "Creates the short version of a URL and returns the result. If no vanity is specified one will be automatically generated for you.", Visibility = OpenApiVisibilityType.Important)]
        //[OpenApiParameter(name: "shortUrl", In = ParameterLocation.Path, Required = true, Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
        //[OpenApiResponseWithoutBody(System.Net.HttpStatusCode.Redirect)]
        [FunctionName("UrlRedirect")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "UrlRedirect/{shortUrl}")] HttpRequestMessage req,
            string shortUrl,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed for Url: {shortUrl}");

            string redirectUrl = "https://azure.com";

            if (!String.IsNullOrWhiteSpace(shortUrl))
            {
                redirectUrl = Environment.GetEnvironmentVariable("defaultRedirectUrl");

                StorageTableHelper stgHelper = new StorageTableHelper(Environment.GetEnvironmentVariable("UlsDataStorage"));

                var tempUrl = new ShortUrlEntity(string.Empty, shortUrl);

                var newUrl = await stgHelper.GetShortUrlEntity(tempUrl);

                if (newUrl != null)
                {
                    //log.LogInformation($"Found it: {newUrl.Url}");
                    newUrl.Clicks++;
                    stgHelper.SaveClickStatsEntity(new ClickStatsEntity(newUrl.RowKey));
                    await stgHelper.SaveShortUrlEntity(newUrl);
                    redirectUrl = newUrl.Url;
                }
            }
            else
            {
                log.LogInformation("Bad Link, resorting to fallback.");
            }

            return new RedirectResult(redirectUrl);
        }
    }
}
