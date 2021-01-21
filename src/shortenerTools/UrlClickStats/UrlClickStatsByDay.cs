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
using System.Text.Json;
using System.IO;
using System.Linq;

namespace Cloud5mins.Function
{
    public static class UrlClickStatsByDay
    {
        [FunctionName("UrlClickStatsByDay")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log,
        ExecutionContext context,
        ClaimsPrincipal principal)
        {
            log.LogInformation($"C# HTTP trigger function processed this request: {req}");

            string userId = string.Empty;
            UrlClickStatsRequest input;
            var result = new ClickDateList();

            var invalidRequest = Utility.CatchUnauthorize(principal, log);
            if (invalidRequest != null)
            {
                return invalidRequest;
            }
            else
            {
                userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                log.LogInformation("Authenticated user {user}.", userId);
            }

            // Validation of the inputs
            if (req == null)
            {
                return new BadRequestObjectResult(new { StatusCode = HttpStatusCode.NotFound });
            }

            try
            {

                using (var reader = new StreamReader(req.Body))
                {
                    var strBody = reader.ReadToEnd();
                    input = JsonSerializer.Deserialize<UrlClickStatsRequest>(strBody, new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
                    if (input == null)
                    {
                        return new BadRequestObjectResult(new { StatusCode = HttpStatusCode.NotFound });
                    }
                }

                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                StorageTableHelper stgHelper = new StorageTableHelper(config["UlsDataStorage"]);

                // string searchingKey = "";
                // log.LogInformation($"Fetching for {input.Vanity}.");
                // if(!string.IsNullOrEmpty(input.Vanity)){
                //     searchingKey = input.Vanity;
                // }
                // log.LogInformation($"Now Looking for {searchingKey}.");

                var rawStats = await stgHelper.GetAllStatsByVanity(input.Vanity);
                

                result.Items = rawStats.GroupBy( s => DateTime.Parse(s.Datetime).Date)
                                            .Select(stat => new ClickDate{
                                                DateClicked = stat.Key.ToString("yyyy-MM-dd"),
                                                Count = stat.Count()
                                            }).OrderBy(s => DateTime.Parse(s.DateClicked).Date).ToList<ClickDate>(); 

                var host = string.IsNullOrEmpty(config["customDomain"]) ? req.Host.Host: config["customDomain"].ToString();
                result.Url = Utility.GetShortUrl(host, input.Vanity);
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
