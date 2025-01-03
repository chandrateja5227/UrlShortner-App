namespace UrlShortner.Server.Interfaces
{
    public interface ISearchService
    {
        Task<string> GetLongUrlAsync(string shortCode);
        Task<bool> SaveUrlMappingAsync(string shortCode, string longUrl);
        Task IncrementClickCountAsync(string shortCode);
        Task<IEnumerable<string>> SearchUrlsAsync(string searchTerm);
    }
}
