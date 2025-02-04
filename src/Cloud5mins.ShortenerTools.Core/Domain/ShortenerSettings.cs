namespace Cloud5mins.ShortenerTools.Core.Domain
{
    public class ShortenerSettings
    {
        public string DefaultRedirectUrl { get; set; }
        public string CustomDomain { get; set; }
        public string DataStorage { get; set; }
        public string BlobStorageConnectionString { get; set; }
        public string BlobStorageContainer { get; set; }
    }
}