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

using Cloud5mins.domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using shortenerTools.Abstractions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cloud5mins.Function
{
    public class UrlList
    {
        private readonly IStorageTableHelper _storageTableHelper;

        public UrlList(IStorageTableHelper storageTableHelper)
        {
            _storageTableHelper = storageTableHelper;
        }

        [FunctionName("UrlList")]
        public async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestMessage req,
        ILogger log,
        ExecutionContext context)
        {
            log.LogInformation($"C# HTTP trigger function processed this request: {req}");

            var result = new ListResponse();

            try
            {
                result.UrlList = await _storageTableHelper.GetAllShortUrlEntities();
                result.UrlList = result.UrlList.Where(p => !(p.IsArchived ?? false)).ToList();
                var host = req.RequestUri.GetLeftPart(UriPartial.Authority);
                foreach (var url in result.UrlList)
                {
                    url.ShortUrl = Utility.GetShortUrl(host, url.RowKey);
                }
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
