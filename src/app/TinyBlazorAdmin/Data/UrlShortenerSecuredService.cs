using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;


namespace TinyBlazorAdmin.Data
{
    /// <summary>
    /// Client to fetch the Cosmos DB token.
    /// </summary>
    public class UrlShortenerSecuredService
    {
        /// <summary>
        /// An <see cref="HttpClient"/> instance configured to securely access
        /// the functions endpoint.
        /// </summary>
        private readonly HttpClient _client;

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

        /// <summary>
        /// Creates a new instance of the <see cref="UrlShortenerSecuredService"/> class.
        /// </summary>
        /// <param name="factory"></param>
        public UrlShortenerSecuredService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(nameof(UrlShortenerSecuredService));
        }

        public async Task<ShortUrlList> GetUrlList()
        {
            string result = string.Empty;
            var resultList = await _client.GetFromJsonAsync<ShortUrlList>($"/api/UrlList");
            return resultList;
        }

        public async Task<ShortUrlList> CreateShortUrl(ShortUrlRequest shortUrlRequest)
        {
            CancellationToken cancellationToken = new CancellationToken();

            var response = await _client.PostAsJsonAsync($"/api/UrlShortener", shortUrlRequest, cancellationToken);

            var resultList = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ShortUrlList>(resultList);
        }

        public async Task<ShortUrlEntity> UpdateShortUrl(ShortUrlEntity editedUrl)
        {
            CancellationToken cancellationToken = new CancellationToken();

            var response = await _client.PostAsJsonAsync($"/api/UrlUpdate", editedUrl, cancellationToken);

            var resultList = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ShortUrlEntity>(resultList);
        }

        public async Task<ShortUrlEntity> ArchiveShortUrl(ShortUrlEntity archivedUrl)
        {
            CancellationToken cancellationToken = new CancellationToken();

            var response = await _client.PostAsJsonAsync($"/api/UrlArchive", archivedUrl, cancellationToken);

            var resultList = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ShortUrlEntity>(resultList);
        }

        public async Task<ClickDateList> GetClickStats(string vanity) {
            try{
            CancellationToken cancellationToken = new CancellationToken();

            string result = string.Empty;
            var response = await _client.PostAsJsonAsync($"/api/UrlClickStatsByDay", new { Vanity = vanity }, cancellationToken);
            var resultList = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ClickDateList>(resultList);;

            }
            catch(Exception ex){
                var ttt = ex.Message;
                return new ClickDateList();
            }    
        }
    }
}