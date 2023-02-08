namespace Cloud5mins.ShortenerTools.Domain
{
    public class ShortRequest
    {
        public string Vanity { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public Schedule[] Schedules { get; set; }
    }
}