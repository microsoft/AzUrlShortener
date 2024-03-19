/*
```c#
Input:

    {
        // [Required] The url you wish to have a short version for
        "url": "https://docs.microsoft.com/en-ca/azure/azure-functions/functions-create-your-first-function-visual-studio",
        
        // [Optional] Title of the page, or text description of your choice.
        "title": "Quickstart: Create your first function in Azure using Visual Studio"

        // [Optional] the end of the URL. If nothing one will be generated for you.
        "vanity": "azFunc"
    }

Output:
    {
        "ShortUrl": "http://c5m.ca/azFunc",
        "LongUrl": "https://docs.microsoft.com/en-ca/azure/azure-functions/functions-create-your-first-function-visual-studio"
    }
*/

using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Messages;
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

    public class UrlCreate
    {
        private readonly ILogger _logger;
        private readonly ShortenerSettings _settings;

        public UrlCreate(ILoggerFactory loggerFactory, ShortenerSettings settings)
        {
            _logger = loggerFactory.CreateLogger<UrlList>();
            _settings = settings;
        }

        [Function("UrlCreate")]
        public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "UrlCreate")] HttpRequestData req,
            ExecutionContext context
        )
        {
            _logger.LogInformation($"__trace creating shortURL: {req}");
            ShortResponse result;
            try
            {
                // Validation of the inputs
                ShortRequest input = await InputValidator.ValidateShortRequest(req);
                if(input == null)
                {
                    return req.CreateResponse(HttpStatusCode.NotFound);
                }

                var host = string.IsNullOrEmpty(_settings.CustomDomain) ? req.Url.Host : _settings.CustomDomain;
                var urlServices = new UrlServices(_settings, _logger);
                result = await urlServices.Create(input, host);
            }
            catch (ShortenerToolException shortEx)
            {
                var badResponse = req.CreateResponse(shortEx.StatusCode);
                await badResponse.WriteAsJsonAsync(new { shortEx.Message });
                return badResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error was encountered.");

                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(new { ex.Message });
                return badResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
