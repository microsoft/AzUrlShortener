using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cloud5mins.domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Cloud5mins.Function
{
    public class Initialize
    {
        private readonly ILogger _logger;
        private readonly ShortenerSettings _shortenerSettings;

        public Initialize(ILoggerFactory loggerFactory, ShortenerSettings settings)
        {
            _logger = loggerFactory.CreateLogger<Initialize>();
            _shortenerSettings = settings;
        }

        [Function("Init")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequestData req,
            ExecutionContext context)
        {

            StorageTableHelper stgHelper = new StorageTableHelper(_shortenerSettings.UlsDataStorage);
            stgHelper.CreateTables();
            return req.CreateResponse(HttpStatusCode.OK);

        }
    }
}
