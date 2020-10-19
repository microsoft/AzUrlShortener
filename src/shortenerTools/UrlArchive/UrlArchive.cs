/*
```c#
Input:
    {
         // [Required]
        "PartitionKey": "d",

         // [Required]
        "RowKey": "doc",

    }


*/

using Cloud5mins.domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using shortenerTools.Abstractions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cloud5mins.Function
{
    public class UrlArchive
    {
        private readonly IStorageTableHelper _storageTableHelper;

        public UrlArchive(IStorageTableHelper storageTableHelper)
        {
            _storageTableHelper = storageTableHelper;
        }

        [FunctionName("UrlArchive")]
        public async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage req,
        ILogger log,
        ExecutionContext context)
        {
            log.LogInformation($"C# HTTP trigger function processed this request: {req}");

            // Validation of the inputs
            if (req == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var input = await req.Content.ReadAsAsync<ShortUrlEntity>();
            if (input == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            ShortUrlEntity result;
            
            try
            {
                result = await _storageTableHelper.ArchiveShortUrlEntity(input);
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
