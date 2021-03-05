using System;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace Cloud5mins.domain
{
    public class ShortUrlEntity : TableEntity
    {
        //public string Id { get; set; }
        private string _url { get; set; }

        public string Url { 
            get{
                return GetUrl();
            }
            set {
                _url = value;
            }
        }


        public string Title { get; set; }

        public string ShortUrl { get; set; }

        public int Clicks { get; set; }

        public bool? IsArchived { get; set; }

        public Schedule[] Schedules { get; set; }

        public ShortUrlEntity(){}

        public ShortUrlEntity(string longUrl, string endUrl)
        {
            Initialize(longUrl, endUrl, string.Empty, null);
        }

        public ShortUrlEntity(string longUrl, string endUrl, Schedule[] schedules)
        {
            Initialize(longUrl, endUrl, string.Empty, schedules);
        }

        public ShortUrlEntity(string longUrl, string endUrl, string title, Schedule[] schedules)
        {
            Initialize(longUrl, endUrl, title, schedules);
        }

        private void Initialize(string longUrl, string endUrl, string title, Schedule[] schedules)
        {
            PartitionKey = endUrl.First().ToString();
            RowKey = endUrl;
            _url = longUrl;
            Title = title;
            Clicks = 0;
            IsArchived = false;
            Schedules = schedules;
        }

        public static ShortUrlEntity GetEntity(string longUrl, string endUrl, string title, Schedule[] schedules){
            return new ShortUrlEntity
            {
                PartitionKey = endUrl.First().ToString(),
                RowKey = endUrl,
                _url = longUrl,
                Title = title,
                Schedules = schedules
            };
        }

        private string GetUrl()
        {
            return GetUrl(DateTime.UtcNow);
        }
        private string GetUrl(DateTime pointInTime)
        {
            var link = _url;
            //var now = DateTime.UtcNow;
            var active = Schedules.Where(s =>
                s.End > pointInTime && //hasn't ended
                s.Start < pointInTime //already started
                ).OrderBy(s => s.Start); //order by start to process first link

            foreach (var sched in active.ToArray())
            {
                if (sched.IsActive(pointInTime))
                {
                    link = sched.AlternativeUrl;
                    break;
                }
            }

            return link;
        }
    }


}
