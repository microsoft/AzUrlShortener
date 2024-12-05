using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace Cloud5mins.Function
{
    public class UrlQRCodeCreate
    {
        private readonly ILogger _logger;
        private readonly ShortenerSettings _settings;

        public UrlQRCodeCreate(ILoggerFactory loggerFactory, ShortenerSettings settings)
        {
            _logger = loggerFactory.CreateLogger<UrlQRCodeCreate>();
            _settings = settings;
        }

        [Function("UrlQRCodeCreate")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "UrlQRCodeCreate/{shortUrl}")] 
            HttpRequestData req,
            string shortUrl, 
            ExecutionContext context) //,
            //ILogger log)
        {

           //log.LogInformation($"C# HTTP trigger function processed for Url: {shortUrl}");

           var redirectUrl = "http://api.qrserver.com/v1/create-qr-code/?color=000000&bgcolor=FFFFFF&data="+WebUtility.UrlEncode(req.RequestUri.AbsoluteUri)+"&qzone=0&margin=0&size=250x250&ecc=L";

           var res = req.CreateResponse(HttpStatusCode.Redirect);
           res.Headers.Add("Location", redirectUrl);
           return res;
        }
    }
}