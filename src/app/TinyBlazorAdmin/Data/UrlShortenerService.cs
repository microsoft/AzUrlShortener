using Microsoft.Extensions.Configuration;

namespace tinyBlazorAdmin.Data
{
    public class UrlShortenerService
    {
        public IConfiguration Config { get; set; }

        public UrlShortenerService(IConfiguration config)
        {
            //Config = GetConfiguration();
            Config = config;
        }

        // private async Task<string> GetFunctionUrl(string functionName){
        //     StringBuilder FuncUrl = new StringBuilder(await GetSecret("AzFunctionURL"));
        //     FuncUrl.Append("/api/");
        //     FuncUrl.Append(functionName);

        //     string code = await GetSecret("AzFunctionCode");
        //     if(!string.IsNullOrWhiteSpace(code))
        //     {
        //         FuncUrl.Append("?code=");
        //         FuncUrl.Append(code);
        //     }

        //     return FuncUrl.ToString();
        // }

        // public async Task<ShortUrlList> GetUrlList()
        // {
        //     var url = await GetFunctionUrl("UrlList");

        //     CancellationToken cancellationToken;

        //     using (var client = new HttpClient())
        //     using (var request = new HttpRequestMessage(HttpMethod.Get, url))
        //     {
        //         using (var response = await client
        //             .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
        //             .ConfigureAwait(false))
        //         {
        //             var resultList = response.Content.ReadAsStringAsync().Result;
        //             return JsonConvert.DeserializeObject<ShortUrlList>(resultList);
        //         }
        //     }
        // }

        // private static StringContent CreateHttpContent(object content)
        // {
        //     StringContent httpContent = null;

        //     if (content != null)
        //     {
        //         var jsonString = JsonConvert.SerializeObject(content);
        //         httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        //     }

        //     return httpContent;
        // }

        // public static void SerializeJsonIntoStream(object value, Stream stream)
        // {
        //     using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
        //     using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
        //     {
        //         var js = new JsonSerializer();
        //         js.Serialize(jtw, value);
        //         jtw.Flush();
        //     }
        // }
    }
}