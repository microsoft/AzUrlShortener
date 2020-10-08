using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using Cloud5mins.domain;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace Cloud5mins.Function
{
    public static class UrlQRRedirect
    {
        [FunctionName("UrlQRRedirect")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "UrlQRRedirect/{shortUrl}")] HttpRequestMessage req,
            string shortUrl, 
            ExecutionContext context,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed for Url: {shortUrl}");

            string redirectUrl = "https://azure.com";

            if (!String.IsNullOrWhiteSpace(shortUrl))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                redirectUrl = config["defaultRedirectUrl"];

                StorageTableHelper stgHelper = new StorageTableHelper(config["UlsDataStorage"]); 

                var tempUrl = new ShortUrlEntity(string.Empty, shortUrl);
                
                var newUrl = await stgHelper.GetShortUrlEntity(tempUrl);

                if (newUrl != null)
                {
                    log.LogInformation($"Found it: {newUrl.Url}");
                    newUrl.Clicks++;
                    stgHelper.SaveClickStatsEntity(new ClickStatsEntity(newUrl.RowKey));
                    await stgHelper.SaveShortUrlEntity(newUrl);
                    redirectUrl = newUrl.Url
                    ;
                }
            }
            else
            {
                log.LogInformation("Bad Link, resorting to fallback.");
            }

            string html =     "<html>"
                            + "<head><title>QR-Code</title></head>"
                            + "<body>"
                            + "<h1>QR-Code</h1>"
                            + "<div id=\"placeholder\"></div>"
                            + "<script type=\"text/javascript\">\n"
	                        + "/* <![CDATA[ */\n"
                            + "content = \"<p>QR for Short-URL: \"+window.location.href+\"<br />\n\";\n"
                            + "content += \"// Embadding from http://goqr.me/\";\n"
                            + "content += \"<img src=\\\"http://api.qrserver.com/v1/create-qr-code/?color=000000&amp;bgcolor=FFFFFF&amp;data=\"+encodeURI(window.location.href)+\"&amp;qzone=0&amp;margin=0&amp;size=500x500&amp;ecc=L\\\" alt=\\\"qr code\\\" />\";\n"
                            + "content += \"//\"\n" 
                            + "document.getElementById('placeholder').innerHTML = content;"
                            + "/* ]]> */"
                        	+ "</script>"
                            + "</p><p>QR for Long-URL: "+WebUtility.HtmlEncode(redirectUrl)+"<br />"
                            // Embadding from http://goqr.me/
                            + "<img src=\"http://api.qrserver.com/v1/create-qr-code/?color=000000&amp;bgcolor=FFFFFF&amp;data="+WebUtility.UrlEncode(redirectUrl)+"&amp;qzone=0&amp;margin=0&amp;size=500x500&amp;ecc=L\" alt=\"qr code\" />"
                            // 
                            + "</p></body>"
                            + "</html>";

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(html);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;

        }
  }
}
