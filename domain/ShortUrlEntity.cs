using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos.Table;

namespace Cloud5mins.domain
{
	public class ShortUrlEntity : TableEntity
	{
		public string Url { get; set; }
		private string _activeUrl { get; set; }

		public string ActiveUrl
		{
			get
			{
				if (String.IsNullOrEmpty(_activeUrl))
					_activeUrl = GetActiveUrl();
				return _activeUrl;
			}
		}


		public string Title { get; set; }

		public string ShortUrl { get; set; }

		public int Clicks { get; set; }

		public bool? IsArchived { get; set; }
		public string SchedulesPropertyRaw { get; set; }

		[IgnoreProperty]
		public Schedule[] Schedules
		{
			get
			{
				if (String.IsNullOrEmpty(SchedulesPropertyRaw))
					return null;
				return JsonSerializer.Deserialize<Schedule[]>(SchedulesPropertyRaw);
			}
			set
			{
				SchedulesPropertyRaw = JsonSerializer.Serialize(value);
			}
		}

		public ShortUrlEntity() { }

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
			Url = longUrl;
			Title = title;
			Clicks = 0;
			IsArchived = false;
			Schedules = schedules;
		}

		public static ShortUrlEntity GetEntity(string longUrl, string endUrl, string title, Schedule[] schedules)
		{
			return new ShortUrlEntity
			{
				PartitionKey = endUrl.First().ToString(),
				RowKey = endUrl,
				Url = longUrl,
				Title = title,
				Schedules = schedules
			};
		}

		private string GetActiveUrl()
		{
			if (Schedules != null)
				return GetActiveUrl(DateTime.UtcNow);
			return Url;
		}
		private string GetActiveUrl(DateTime pointInTime)
		{
			var link = Url;
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