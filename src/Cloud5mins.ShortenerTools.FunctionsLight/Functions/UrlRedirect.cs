using Cloud5mins.ShortenerTools.Core.Services;
using Cloud5mins.ShortenerTools.Core.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cloud5mins.ShortenerTools.Core.Service;
using Azure.Data.Tables;

namespace Cloud5mins.ShortenerTools.Functions
{
    public class UrlRedirect
    {
        private readonly ILogger _logger;
        private TableServiceClient _tblClient;

        public UrlRedirect(ILoggerFactory loggerFactory, TableServiceClient tblClient)
        {
            _logger = loggerFactory.CreateLogger<UrlRedirect>();
            _logger.LogInformation("UrlRedirect in constructor");
            _tblClient = tblClient;
        }

        [Function("UrlRedirect")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{shortUrl}")]
            HttpRequestData req,
            string shortUrl,
            ExecutionContext context)
        {
            _logger.LogInformation("Function reached");
            UrlServices UrlServices = new UrlServices(_logger, new AzStrorageTablesService(_tblClient));
            _logger.LogInformation("Services created");
            _logger.LogInformation($"Redirecting {shortUrl}");
            string redirectUrl = await UrlServices.Redirect(shortUrl);
            _logger.LogInformation("got the redirect url");
            var res = req.CreateResponse(HttpStatusCode.Redirect);
            res.Headers.Add("Location", redirectUrl);
            _logger.LogInformation("response created");
            return res;

        }
    }
}
