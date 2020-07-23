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

using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using Cloud5mins.domain;
using Microsoft.Extensions.Configuration;

namespace Cloud5mins.Function
{

    public static class UrlShortener
    {

        [FunctionName("UrlShortener")]
        public static async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, 
        ILogger log, 
        ExecutionContext context)
        {
            log.LogInformation($"C# HTTP trigger function processed this request: {req}");

            // Validation of the inputs
            if (req == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            ShortRequest input = await req.Content.ReadAsAsync<ShortRequest>();
            if (input == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }


            // If the Url parameter only contains whitespaces or is empty return with BadRequest.
            if (string.IsNullOrWhiteSpace(input.Url))
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "The url parameter can not be empty.");
            }

            // Validates if input.url is a valid aboslute url, aka is a complete refrence to the resource, ex: http(s)://google.com
            if (!Uri.IsWellFormedUriString(input.Url, UriKind.Absolute))
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, $"{input.Url} is not a valid absolute Url. The Url parameter must start with 'http://' or 'http://'.");
            }
            
            var result = new ShortResponse();
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            StorageTableHelper stgHelper = new StorageTableHelper(config["UlsDataStorage"]); 

            try
            {
                string longUrl = input.Url.Trim();
                string vanity = string.IsNullOrWhiteSpace(input.Vanity) ? "" : input.Vanity.Trim();
                string title = string.IsNullOrWhiteSpace(input.Title) ? "" : input.Title.Trim();

                ShortUrlEntity newRow;

                if(!string.IsNullOrEmpty(vanity))
                {
                    newRow = new ShortUrlEntity(longUrl, vanity, title);
                    if(await stgHelper.IfShortUrlEntityExist(newRow))
                    {
                        return req.CreateResponse(HttpStatusCode.Conflict, "This Short URL already exist.");
                    }
                }
                else
                {
                    newRow = new ShortUrlEntity(longUrl, await Utility.GetValidEndUrl(vanity, stgHelper), title);
                }

                await stgHelper.SaveShortUrlEntity(newRow);

                var host = req.RequestUri.GetLeftPart(UriPartial.Authority);
                log.LogInformation($"-> host = {host}");
                result = new ShortResponse(host, newRow.Url, newRow.RowKey, newRow.Title);

                log.LogInformation("Short Url created.");
             }
            catch (Exception ex)
            {
                log.LogError(ex, "An unexpected error was encountered.");
                return req.CreateResponse(HttpStatusCode.BadRequest, ex);
            }

            return req.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
