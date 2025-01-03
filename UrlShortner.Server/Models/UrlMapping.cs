namespace UrlShortner.Server.Models
{
    public class UrlMapping
    {
        public int Id { get; set; }
        public string ShortCode { get; set; }
        public string LongUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public long ClickCount { get; set; }
    }
}
