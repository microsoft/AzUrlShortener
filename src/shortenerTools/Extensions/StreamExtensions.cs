using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace shortenerTools.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<T> DeserializeJsonFromStreamAsync<T>(this Stream stream)
        {
            if (!(stream is {CanRead: true}))
                return default;

            using var sr = new StreamReader(stream);
            var searchResult = await JsonSerializer.DeserializeAsync<T>(sr.BaseStream);
            return searchResult;
        }

        public static async Task<string> StreamToStringAsync(this Stream stream)
        {
            if (stream == null)
                return null;

            using var sr = new StreamReader(stream);
            var content = await sr.ReadToEndAsync();

            return content;
        }
    }
}