/*
```c#
Input:

    {
        // [Required] the end of the URL that you want statistics for.
        "vanity": "azFunc"
    }

Output:
    {
    "items": [
        {
        "dateClicked": "2020-12-19",
        "count": 1
        },
        {
        "dateClicked": "2020-12-03",
        "count": 2
        }
    ],
    "url": ""https://c5m.ca/29"
*/

using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Messages;
using Cloud5mins.ShortenerTools.Core.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud5mins.ShortenerTools.Functions
{
    public class UrlClickStatsByDay
    {
        private readonly ILogger _logger;
        private readonly ShortenerSettings _settings;

        public UrlClickStatsByDay(ILoggerFactory loggerFactory, ShortenerSettings settings)
        {
            _logger = loggerFactory.CreateLogger<UrlClickStatsByDay>();
            _settings = settings;
        }

        [Function("UrlClickStatsByDay")]
        public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "UrlClickStatsByDay")] HttpRequestData req,
        ExecutionContext context)
        {
            _logger.LogInformation($"HTTP trigger: UrlClickStatsByDay");

            var result = new ClickDateList();

            UrlClickStatsRequest input = await InputValidator.ValidateUrlClickStatsRequest(req);
            if(input == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            
            try
            {
                var host = string.IsNullOrEmpty(_settings.CustomDomain) ? req.Url.Host : _settings.CustomDomain.ToString();
                var urlServices = new UrlServices(_settings, _logger);
                result = await urlServices.ClickStatsByDay(input, host);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error was encountered.");
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteAsJsonAsync(new { Message = $"{ex.Message}" });
                return badRequest;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);
            return response;
        }
    }
}
