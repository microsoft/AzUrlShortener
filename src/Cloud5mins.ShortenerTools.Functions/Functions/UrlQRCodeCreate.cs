using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Messages;

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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "api/UrlQRCodeCreate")] HttpRequestData req,
            ExecutionContext context)
        {
           //log.LogInformation($"C# HTTP trigger function processed for Url: {shortUrl}");

           ShortRequest input;

           // Validation of the inputs
            if (req == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            using (var reader = new StreamReader(req.Body))
            {
                var strBody = await reader.ReadToEndAsync();
                input = JsonSerializer.Deserialize<ShortRequest>(strBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (input == null)
                {
                    return req.CreateResponse(HttpStatusCode.NotFound);
                }
            }

           var redirectUrl = "https://api.qrserver.com/v1/create-qr-code/?color=000000&bgcolor=FFFFFF&data="+WebUtility.UrlEncode(input.Url)+"&qzone=0&margin=0&size=250x250&ecc=L";

            req.FunctionContext.GetHttpResponseData()?.Headers.Add("Access-Control-Allow-Origin", "https://kmpl.fun");
      
            req.Headers.Add("Access-Control-Allow-Origin", "https://kmpl.fun");
           var res = req.CreateResponse(HttpStatusCode.Redirect);
           res.Headers.Add("Location", redirectUrl);
           res.Headers.Add("Access-Control-Allow-Origin", "https://kmpl.fun");
           return res;
        }
    }
}