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

using Cloud5mins.ShortenerTools.Core.Domain;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud5mins.ShortenerTools.Functions
{
    public class UrlUpdate
    {
        private readonly ILogger _logger;
        private readonly ShortenerSettings _settings;

        public UrlUpdate(ILoggerFactory loggerFactory, ShortenerSettings settings)
        {
            _logger = loggerFactory.CreateLogger<UrlList>();
            _settings = settings;
        }

        [Function("UrlUpdate")]
        public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "api/UrlUpdate")] HttpRequestData req,
                                    ExecutionContext context
                                )
        {
            _logger.LogInformation($"HTTP trigger - UrlUpdate");

            string userId = string.Empty;
            ShortUrlEntity input;
            ShortUrlEntity result;

            try
            {
                // Validation of the inputs
                if (req == null)
                {
                    return req.CreateResponse(HttpStatusCode.NotFound);
                }

                using (var reader = new StreamReader(req.Body))
                {
                    var strBody = await reader.ReadToEndAsync();
                    input = JsonSerializer.Deserialize<ShortUrlEntity>(strBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (input == null)
                    {
                        return req.CreateResponse(HttpStatusCode.NotFound);
                    }
                }

                // If the Url parameter only contains whitespaces or is empty return with BadRequest.
                if (string.IsNullOrWhiteSpace(input.Url))
                {
                    var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badRequest.WriteAsJsonAsync(new { Message = "The url parameter can not be empty." });
                    return badRequest;
                }

                // Validates if input.url is a valid aboslute url, aka is a complete refrence to the resource, ex: http(s)://google.com
                if (!Uri.IsWellFormedUriString(input.Url, UriKind.Absolute))
                {
                    var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badRequest.WriteAsJsonAsync(new { Message = $"{input.Url} is not a valid absolute Url. The Url parameter must start with 'http://' or 'http://'." });
                    return badRequest;
                }

                StorageTableHelper stgHelper = new StorageTableHelper(_settings.DataStorage);

                result = await stgHelper.UpdateShortUrlEntity(input);
                var host = string.IsNullOrEmpty(_settings.CustomDomain) ? req.Url.Host : _settings.CustomDomain.ToString();
                result.ShortUrl = Utility.GetShortUrl(host, result.RowKey);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error was encountered.");

                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteAsJsonAsync(new { ex.Message });
                return badRequest;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);
            return response;
        }
    }
}