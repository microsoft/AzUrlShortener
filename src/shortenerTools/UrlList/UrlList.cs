/*
```c#
Input:


Output:
    {
        "Url": "https://SOME_URL",
        "Clicks": 0,
        "PartitionKey": "d",
        "title": "Quickstart: Create your first function in Azure using Visual Studio"
        "RowKey": "doc",
        "Timestamp": "0001-01-01T00:00:00+00:00",
        "ETag": "W/\"datetime'2020-05-06T14%3A33%3A51.2639969Z'\""
    }
*/

using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Cloud5mins.domain;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Cloud5mins.Function
{
    public static class UrlList
    {
        [FunctionName("UrlList")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        ILogger log,
        ExecutionContext context,
        ClaimsPrincipal principal)
        {
            log.LogInformation($"C# HTTP trigger function processed this request: {req}");

            var result = new ListResponse();
            string userId = string.Empty;
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            StorageTableHelper stgHelper = new StorageTableHelper(config["UlsDataStorage"]);

            try
            {
                var invalidRequest = Utility.CatchUnauthorize(principal, log);
                if (invalidRequest != null)
                {
                    return invalidRequest;
                }
                else
                {
                   userId = principal.FindFirst(ClaimTypes.GivenName).Value;
                   log.LogInformation("Authenticated user {user}.", userId);
                }

                result.UrlList = await stgHelper.GetAllShortUrlEntities();
                result.UrlList = result.UrlList.Where(p => !(p.IsArchived ?? false)).ToList();
                var host = string.IsNullOrEmpty(config["customDomain"]) ? req.Host.Host: config["customDomain"].ToString();
                foreach (ShortUrlEntity url in result.UrlList)
                {
                    url.ShortUrl = Utility.GetShortUrl(host, url.RowKey);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error was encountered.");
                return new BadRequestObjectResult(new
                {
                    message = ex.Message,
                    StatusCode =  HttpStatusCode.BadRequest
                });
            }

            return new OkObjectResult(result);
        }
    }
}
