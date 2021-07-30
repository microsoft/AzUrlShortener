using adminBlazorWebsite.Abstractions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace adminBlazorWebsite.Data
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly HttpClient _httpClient;
        public IConfigurationRoot Config { get; set; }

        public UrlShortenerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Config = GetConfiguration();
        }

        public async Task<ShortUrlList> GetUrlList()
        {
            var url = GetFunctionUrl("UrlList");

            CancellationToken cancellationToken;

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _httpClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
            var resultList = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ShortUrlList>(resultList);
        }

        public async Task<ShortUrlList> CreateShortUrl(ShortUrlRequest shortUrlRequest)
        {
            return await SendAsync<ShortUrlList, ShortUrlRequest>(shortUrlRequest, "UrlShortener", HttpMethod.Post);
        }

        public async Task<ShortUrlEntity> UpdateShortUrl(ShortUrlEntity editedUrl)
        {
            return await SendAsync(editedUrl, "UrlUpdate", HttpMethod.Post);
        }

        public async Task<ShortUrlEntity> ArchiveShortUrl(ShortUrlEntity archivedUrl)
        {
            return await SendAsync(archivedUrl, "UrlArchive", HttpMethod.Post);
        }

        private async Task<TOut> SendAsync<TOut, TIn>(TIn entity, string functionName, HttpMethod method)
        {
            var url = GetFunctionUrl(functionName);

            CancellationToken cancellationToken;

            using var request = new HttpRequestMessage(method, url);
            using var httpContent = CreateHttpContent(entity);
            request.Content = httpContent;

            using var response = await _httpClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
            var resultList = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TOut>(resultList);
        }

        private async Task<T> SendAsync<T>(T entity, string functionName, HttpMethod method)
        {
            return await SendAsync<T, T>(entity, functionName, method);
        }

        private static string GetFunctionUrl(string functionName)
        {
            var funcUrl = new StringBuilder();
            funcUrl.Append(functionName);

            var code = GetConfiguration()["code"];
            if (string.IsNullOrWhiteSpace(code)) return funcUrl.ToString();

            funcUrl.Append("?code=");
            funcUrl.Append(code);

            return funcUrl.ToString();
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

        private static StringContent CreateHttpContent(object content)
        {
            if (content == null) return null;

            var jsonString = JsonConvert.SerializeObject(content);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            return httpContent;
        }
    }
}
