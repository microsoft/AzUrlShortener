using adminBlazorWebsite.Data;
using System.Threading.Tasks;

namespace adminBlazorWebsite.Abstractions
{
    public interface IUrlShortenerService
    {
        Task<ShortUrlList> CreateShortUrl(ShortUrlRequest shortUrlRequest);
        Task<ShortUrlEntity> UpdateShortUrl(ShortUrlEntity editedUrl);
        Task<ShortUrlEntity> ArchiveShortUrl(ShortUrlEntity archivedUrl);
    }
}
