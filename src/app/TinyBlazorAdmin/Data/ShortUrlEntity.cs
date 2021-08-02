using System.Collections.Generic;
using System.Linq;

namespace TinyBlazorAdmin.Data
{
    public class ShortUrlEntity
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string ShortUrl { get; set; }

        public int Clicks { get; set; }

        private List<Schedule> _schedules;

        public List<Schedule> Schedules { 
            get{
                if(_schedules == null){
                    _schedules = new List<Schedule>();
                }
                return _schedules;
            } 
            set{
                _schedules = value;
            } 
        }

        public ShortUrlEntity()
        {
        }

        public static ShortUrlEntity GetEntity(string longUrl, string endUrl)
        {
            return new ShortUrlEntity
            {
                PartitionKey = endUrl.First().ToString(),
                RowKey = endUrl,
                Url = longUrl
            };
        }

        public string GetDisplayableUrl(int max)
        {
            var length = Url.ToString().Length;
            if (length >= max)
            {
                return string.Concat(Url.Substring(0, max-1), "...");
            }
            return Url;
        }
    }
}