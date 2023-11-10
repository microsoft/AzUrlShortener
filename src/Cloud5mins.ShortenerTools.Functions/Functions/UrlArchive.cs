/*
```c#
Input:
    {
         // [Required]
        "PartitionKey": "d",

         // [Required]
        "RowKey": "doc",

        // [Optional] all other properties
    }
Output:
    {
        "Url": "https://docs.microsoft.com/en-ca/azure/azure-functions/functions-create-your-first-function-visual-studio",
        "Title": "My Title",
        "ShortUrl": null,
        "Clicks": 0,
        "IsArchived": true,
        "PartitionKey": "a",
        "RowKey": "azFunc2",
        "Timestamp": "2020-07-23T06:22:33.852218-04:00",
        "ETag": "W/\"datetime'2020-07-23T10%3A24%3A51.3440526Z'\""
    }

*/

using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Services;
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
    public class UrlArchive
    {

        private readonly ILogger _logger;
        private readonly ShortenerSettings _settings;

        public UrlArchive(ILoggerFactory loggerFactory, ShortenerSettings settings)
        {
            _logger = loggerFactory.CreateLogger<UrlList>();
            _settings = settings;
        }

        [Function("UrlArchive")]
        public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "UrlArchive")] HttpRequestData req,
        ExecutionContext context)
        {
            _logger.LogInformation($"HTTP trigger - UrlArchive");

            ShortUrlEntity result;
            try
            {
                // Validation of the inputs
                ShortUrlEntity input = await InputValidator.ValidateShortUrlEntity(req);
                if(input == null)
                {
                    return req.CreateResponse(HttpStatusCode.NotFound);
                }

                var urlServices = new UrlServices(_settings, _logger);
                result = await urlServices.Archive(input);
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
