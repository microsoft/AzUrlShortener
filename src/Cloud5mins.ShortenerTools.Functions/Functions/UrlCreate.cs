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

using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Messages;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace Cloud5mins.ShortenerTools.Functions
{

    public class UrlCreate
    {
        private readonly ILogger _logger;
        private readonly ShortenerSettings _settings;

        public UrlCreate(ILoggerFactory loggerFactory, ShortenerSettings settings)
        {
            _logger = loggerFactory.CreateLogger<UrlList>();
            _settings = settings;
        }

        [Function("UrlCreate")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "api/UrlCreate")] HttpRequestData req,
            ExecutionContext context
        )
        {
            _logger.LogInformation($"__trace creating shortURL: {req}");
            string userId = string.Empty;
            ShortRequest input;
            var result = new ShortResponse();

            try
            {
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

                // If the Url parameter only contains whitespaces or is empty return with BadRequest.
                if (string.IsNullOrWhiteSpace(input.Url))
                {
                    var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badResponse.WriteAsJsonAsync(new { Message = "The url parameter can not be empty." });
                    return badResponse;
                }

                // Validates if input.url is a valid aboslute url, aka is a complete refrence to the resource, ex: http(s)://google.com
                if (!Uri.IsWellFormedUriString(input.Url, UriKind.Absolute))
                {
                    var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badResponse.WriteAsJsonAsync(new { Message = $"{input.Url} is not a valid absolute Url. The Url parameter must start with 'http://' or 'http://'." });
                    return badResponse;
                }

                StorageTableHelper stgHelper = new StorageTableHelper(_settings.DataStorage);

                string longUrl = input.Url.Trim();
                string vanity = string.IsNullOrWhiteSpace(input.Vanity) ? "" : input.Vanity.Trim();
                string title = string.IsNullOrWhiteSpace(input.Title) ? "" : input.Title.Trim();

                ShortUrlEntity newRow;

                if (!string.IsNullOrEmpty(vanity))
                {
                    newRow = new ShortUrlEntity(longUrl, vanity, title, input.Schedules);
                    if (await stgHelper.IfShortUrlEntityExist(newRow))
                    {
                        var badResponse = req.CreateResponse(HttpStatusCode.Conflict);
                        await badResponse.WriteAsJsonAsync(new { Message = "This Short URL already exist." });
                        return badResponse;
                    }
                }
                else
                {
                    newRow = new ShortUrlEntity(longUrl, await Utility.GetValidEndUrl(vanity, stgHelper), title, input.Schedules);
                }

                await stgHelper.SaveShortUrlEntity(newRow);

                var host = string.IsNullOrEmpty(_settings.CustomDomain) ? req.Url.Host : _settings.CustomDomain.ToString();
                var qrCodeUrl = await GetQRCode(newRow.Url);
                result = new ShortResponse(host, newRow.Url, newRow.RowKey, newRow.Title, qrCodeUrl);

                _logger.LogInformation("Short Url created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error was encountered.");

                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(new { ex.Message });
                return badResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        private async Task<string> GetQRCode(string shortUrl)
        {
            // Create the QR Code
            var redirectUrl = "https://api.qrserver.com/v1/create-qr-code/?color=000000&bgcolor=FFFFFF&data=" + WebUtility.UrlEncode(shortUrl) + "&qzone=0&margin=0&size=250x250&ecc=L";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(redirectUrl);
            var content = await response.Content.ReadAsByteArrayAsync();

            // Save file as image on Azure blob storage
            var url = "https://urlshortenerqrcodes.blob.core.windows.net/qr-code-images?sp=r&st=2025-02-03T20:34:26Z&se=2025-02-04T04:34:26Z&spr=https&sv=2022-11-02&sr=c&sig=LZ06Sip3iVq3EmPDGsAtW7unb6RuQdpEkIUNP%2BlBJAg%3D"
            var blobServiceClient = new BlobServiceClient(url);
            var containerClient = blobServiceClient.GetBlobContainerClient("qrcodes");
            var blobClient = containerClient.GetBlobClient($"{Guid.NewGuid()}.png");

            using (var stream = new MemoryStream(content))
            {
                await blobClient.UploadAsync(stream, true);
            }

            // Return the URL to the image
            string qrCodeUrl = blobClient.Uri.ToString();
            _logger.LogInformation("qrCodeUrl: {qrCodeUrl}", qrCodeUrl);
            return qrCodeUrl;
        }
    }
}
