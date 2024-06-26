using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Cloud5mins.domain;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace Cloud5mins.Function
{
    public static class UrlClickStats
    {
        //[OpenApiOperation(operationId: "UrlClickStats", tags: new[] { "Urls" }, Summary = "Get short URL click stats", Description = "Returns statistics for a specific short URL.", Visibility = OpenApiVisibilityType.Important)]
        //[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UrlClickStatsRequest), Required = true)]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ClickStatsEntityList))]
        [FunctionName("UrlClickStats")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, 
        ILogger log, 
        ExecutionContext context)
        {
            log.LogInformation($"C# HTTP trigger function processed this request: {req}");

            // Validation of the inputs
            if (req == null)
            {
                return new NotFoundResult();
            }

            UrlClickStatsRequest input = await req.Content.ReadAsAsync<UrlClickStatsRequest>();
            if (input == null)
            {
                return new NotFoundResult();
            }

            var result = new ClickStatsEntityList();
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            StorageTableHelper stgHelper = new StorageTableHelper(config["UlsDataStorage"]); 

            try
            {
               result.ClickStatsList = await stgHelper.GetAllStatsByVanity(input.Vanity);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error was encountered.");
                return new BadRequestObjectResult(ex);
            }

            return new OkObjectResult(result);
        }
    }
}
