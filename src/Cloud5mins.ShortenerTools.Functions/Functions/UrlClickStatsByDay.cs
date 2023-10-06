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
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "api/UrlClickStatsByDay")] HttpRequestData req,
        ExecutionContext context)
        {
            _logger.LogInformation($"HTTP trigger: UrlClickStatsByDay");

            string userId = string.Empty;
            UrlClickStatsRequest input;
            var result = new ClickDateList();

            // Validation of the inputs
            if (req == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                using (var reader = new StreamReader(req.Body))
                {
                    var strBody = await reader.ReadToEndAsync();
                    input = JsonSerializer.Deserialize<UrlClickStatsRequest>(strBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (input == null)
                    {
                        return req.CreateResponse(HttpStatusCode.NotFound);
                    }
                }

                StorageTableHelper stgHelper = new StorageTableHelper(_settings.DataStorage);

                var rawStats = await stgHelper.GetAllStatsByVanity(input.Vanity);

                result.Items = rawStats.GroupBy(s => DateTime.Parse(s.Datetime).Date)
                                            .Select(stat => new ClickDate
                                            {
                                                DateClicked = stat.Key.ToString("yyyy-MM-dd"),
                                                Count = stat.Count()
                                            }).OrderBy(s => DateTime.Parse(s.DateClicked).Date).ToList<ClickDate>();

                var host = string.IsNullOrEmpty(_settings.CustomDomain) ? req.Url.Host : _settings.CustomDomain.ToString();
                result.Url = Utility.GetShortUrl(host, input.Vanity);
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
