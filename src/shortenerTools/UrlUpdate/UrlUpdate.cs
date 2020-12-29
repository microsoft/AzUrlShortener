/*
```c#
Input:
    {
         // [Required]
        "PartitionKey": "d",

         // [Required]
        "RowKey": "doc",

        // [Optional] New Title for this URL, or text description of your choice.
        "title": "Quickstart: Create your first function in Azure using Visual Studio"

        // [Optional] New long Url where the the user will be redirect
        "Url": "https://SOME_URL"
    }


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
using System.Net.Http;
using Cloud5mins.domain;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;

namespace Cloud5mins.Function
{
    public static class UrlUpdate
    {
        [FunctionName("UrlUpdate")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log,
        ExecutionContext context,
        ClaimsPrincipal principal)
        {
            log.LogInformation($"C# HTTP trigger function processed this request: {req}");

            string userId = string.Empty;
            ShortUrlEntity input;
            ShortUrlEntity result;

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

                // Validation of the inputs
                if (req == null)
                {
                    return new BadRequestObjectResult(new { StatusCode = HttpStatusCode.NotFound });
                }

                using (var reader = new StreamReader(req.Body))
                {
                    var strBody = reader.ReadToEnd();
                    input = JsonSerializer.Deserialize<ShortUrlEntity>(strBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (input == null)
                    {
                        return new BadRequestObjectResult(new { StatusCode = HttpStatusCode.NotFound });
                    }
                }

                // If the Url parameter only contains whitespaces or is empty return with BadRequest.
                if (string.IsNullOrWhiteSpace(input.Url))
                {
                    return new BadRequestObjectResult(new { StatusCode = HttpStatusCode.BadRequest, Message = "The url parameter can not be empty." });
                }

                // Validates if input.url is a valid aboslute url, aka is a complete refrence to the resource, ex: http(s)://google.com
                if (!Uri.IsWellFormedUriString(input.Url, UriKind.Absolute))
                {
                    return new BadRequestObjectResult(new
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = $"{input.Url} is not a valid absolute Url. The Url parameter must start with 'http://' or 'http://'."
                    });
                }

                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                StorageTableHelper stgHelper = new StorageTableHelper(config["UlsDataStorage"]);

                result = await stgHelper.UpdateShortUrlEntity(input);
                var host = string.IsNullOrEmpty(config["customDomain"]) ? req.Host.Host: config["customDomain"].ToString();
                result.ShortUrl = Utility.GetShortUrl(host, result.RowKey);

            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error was encountered.");
                return new BadRequestObjectResult(new
                {
                    message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                });
            }

            return new OkObjectResult(result);
        }
    }
}
