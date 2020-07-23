using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace adminBlazorWebsite.Data
{
    public class UrlShortenerService
    {
        public IConfigurationRoot Config { get; set; }
        public UrlShortenerService()
        {
            Config = GetConfiguration();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            return config;
        }

        private static string GetFunctionUrl(string functionName){
            StringBuilder FuncUrl = new StringBuilder(GetConfiguration()["azFunctionUrl"]);
            FuncUrl.Append("/api/");
            FuncUrl.Append(functionName);

            string code = GetConfiguration()["code"];
            if(!string.IsNullOrWhiteSpace(code))
            {
                FuncUrl.Append("?code=");
                FuncUrl.Append(code);
            }
            
            return FuncUrl.ToString();
        }

        public async Task<ShortUrlList> GetUrlList()
        {
            var url = GetFunctionUrl("UrlList");

            CancellationToken cancellationToken;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (var response = await client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false))
                {
                    var resultList = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<ShortUrlList>(resultList);
                }
            }
        }



        public async Task<ShortUrlList> CreateShortUrl(ShortUrlRequest shortUrlRequest)
        {
            var url = GetFunctionUrl("UrlShortener");

            CancellationToken cancellationToken;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            using (var httpContent = CreateHttpContent(shortUrlRequest))
            {
                request.Content = httpContent;

                using (var response = await client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false))
                {

                    var resultList = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<ShortUrlList>(resultList);
                }
            }
        }


        public async Task<ShortUrlEntity> UpdateShortUrl(ShortUrlEntity editedUrl)
        {
            var url = GetFunctionUrl("UrlUpdate");

            CancellationToken cancellationToken;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            using (var httpContent = CreateHttpContent(editedUrl))
            {
                request.Content = httpContent;

                using (var response = await client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false))
                {

                    var resultList = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<ShortUrlEntity>(resultList);
                }
            }
        }

        public async Task<ShortUrlEntity> ArchiveShortUrl(ShortUrlEntity archivedUrl) 
        {
            var url = GetFunctionUrl("UrlArchive");

            CancellationToken cancellationToken;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            using (var httpContent = CreateHttpContent(archivedUrl))
            {
                request.Content = httpContent;

                using (var response = await client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false))
                {
                    var resultList = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<ShortUrlEntity>(resultList);
                }
            }
        }


        private static StringContent CreateHttpContent(object content)
        {
            StringContent httpContent = null;

            if (content != null)
            {
                var jsonString = JsonConvert.SerializeObject(content);
                httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            }

            return httpContent;
        }

        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }
    }
}
