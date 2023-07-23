using Cloud5mins.ShortenerTools.Core.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud5mins.ShortenerTools.Functions
{
    public class UrlRedirectRoot
    {
        private readonly ILogger _logger;
        private readonly ShortenerSettings _settings;

        public UrlRedirectRoot(ILoggerFactory loggerFactory, ShortenerSettings settings)
        {
            _logger = loggerFactory.CreateLogger<UrlRedirectRoot>();
            _settings = settings;
        }

        [Function("UrlRedirectRoot")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "/")]
            HttpRequestData req,
            ExecutionContext context)
        {
            string redirectUrl = _settings.RootRedirectUrl ?? _settings.DefaultRedirectUrl ?? "https://azure.com";
            var res = req.CreateResponse(HttpStatusCode.Redirect);
            res.Headers.Add("Location", redirectUrl);
            return res;

        }
    }
}
