using UrlShortner.Server.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace UrlShortner.Server.Services
{
    public class UrlService
    {
        private readonly ICacheService _cacheService;
        private readonly ISearchService _searchService;
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int ShortCodeLength = 6;

        public UrlService(ICacheService cacheService, ISearchService searchService)
        {
            _cacheService = cacheService;
            _searchService = searchService;
        }

        public async Task<string> GetLongUrlAsync(string shortCode)
        {
            var cachedUrl = await _cacheService.GetAsync<string>($"url:{shortCode}");
            if (cachedUrl != null)
            {
                await _searchService.IncrementClickCountAsync(shortCode);
                return cachedUrl;
            }

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
            string shortCode;
            int attempts = 0;
            const int maxAttempts = 3;

            do
            {
                shortCode = GenerateShortCode(longUrl, attempts);
                
                if (await _cacheService.ExistsAsync($"url:{shortCode}"))
                    continue;

                var success = await _searchService.SaveUrlMappingAsync(shortCode, longUrl);
                if (success)
                {
                    await _cacheService.SetAsync($"url:{shortCode}", longUrl, TimeSpan.FromHours(24));
                    return shortCode;
                }

                attempts++;
            } while (attempts < maxAttempts);

            throw new Exception("Failed to generate unique short code after maximum attempts");
        }

        private string GenerateShortCode(string longUrl, int attempt)
        {
            var input = $"{longUrl}_{attempt}_{DateTime.UtcNow.Ticks}";
            
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var shortCode = new StringBuilder(ShortCodeLength);

                for (int i = 0; i < ShortCodeLength; i++)
                {
                    shortCode.Append(AllowedChars[hash[i] % AllowedChars.Length]);
                }

                return shortCode.ToString();
            }
        }
    }
}
