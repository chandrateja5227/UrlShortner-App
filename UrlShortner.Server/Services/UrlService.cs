using UrlShortner.Server.Interfaces;

namespace UrlShortner.Server.Services
{
    public class UrlService
    {
        private readonly ICacheService _cacheService;
        private readonly ISearchService _searchService;

        public UrlService(ICacheService cacheService, ISearchService searchService)
        {
            _cacheService = cacheService;
            _searchService = searchService;
        }

        public async Task<string> GetLongUrlAsync(string shortCode)
        {
            // Try cache first
            var cachedUrl = await _cacheService.GetAsync<string>($"url:{shortCode}");
            if (cachedUrl != null)
            {
                await _searchService.IncrementClickCountAsync(shortCode);
                return cachedUrl;
            }

            // If not in cache, get from Solr
            var longUrl = await _searchService.GetLongUrlAsync(shortCode);
            if (longUrl != null)
            {
                await _cacheService.SetAsync($"url:{shortCode}", longUrl, TimeSpan.FromHours(24));
                await _searchService.IncrementClickCountAsync(shortCode);
            }

            return longUrl;
        }

        public async Task<string> CreateShortUrlAsync(string longUrl)
        {
            var shortCode = GenerateShortCode();
            
            // Save to Solr
            var success = await _searchService.SaveUrlMappingAsync(shortCode, longUrl);
            if (!success)
            {
                throw new Exception("Failed to save URL mapping");
            }
            
            // Cache the mapping
            await _cacheService.SetAsync($"url:{shortCode}", longUrl, TimeSpan.FromHours(24));
            
            return shortCode;
        }

        private string GenerateShortCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6);
        }
    }
}
