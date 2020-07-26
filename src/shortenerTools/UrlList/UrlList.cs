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
//using System.Net.Http;
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
                // var invalidRequest = Utility.CatchUnauthorize(principal, log);
                // if (invalidRequest != null)
                // {
                //     return invalidRequest;
                // }
                // else
                // {
                //     userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                //     log.LogInformation("Authenticated user {user}.", userId);
                // }
                if (principal == null)
                {
                    log.LogWarning("No principal.");
                    return new UnauthorizedResult();
                }

                if (principal.Identity == null)
                {
                    log.LogWarning("No identity.");
                    return new UnauthorizedResult();
                }

                if (!principal.Identity.IsAuthenticated)
                {
                    log.LogWarning("Request was not authenticated.");
                    return new UnauthorizedResult();
                }
                userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                log.LogInformation("Authenticated user {user}.", userId);

                result.UrlList = await stgHelper.GetAllShortUrlEntities();
                result.UrlList = result.UrlList.Where(p => !(p.IsArchived ?? false)).ToList();
                //var host = req.HttpContext.RequestUri.GetLeftPart(UriPartial.Authority);
                var host = req.Host.Host;
                foreach (ShortUrlEntity url in result.UrlList)
                {
                    url.ShortUrl = Utility.GetShortUrl(host, url.RowKey);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error was encountered.");
                //return req.CreateResponse(HttpStatusCode.BadRequest, ex);
                return new BadRequestObjectResult(new
                {
                    message = HttpStatusCode.BadRequest,
                    currentDate = DateTime.Now
                });
            }

            //return req.CreateResponse(HttpStatusCode.OK, result);
            return new OkObjectResult(result);
        }
    }
}
