using UrlShortner.Server.Interfaces;
using System.Text.Json;

namespace UrlShortner.Server.Services
{
    public class RedisCacheService : ICacheService
    {
        // TODO: Inject IConnectionMultiplexer in constructor
        
        public async Task<T?> GetAsync<T>(string key)
        {
            // Placeholder: Implement Redis GET
            Console.WriteLine($"Cache GET: {key}");
            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            // Placeholder: Implement Redis SET
            Console.WriteLine($"Cache SET: {key}");
        }

        public async Task RemoveAsync(string key)
        {
            // Placeholder: Implement Redis DEL
            Console.WriteLine($"Cache DEL: {key}");
        }

        public async Task<bool> ExistsAsync(string key)
        {
            // Placeholder: Implement Redis EXISTS
            Console.WriteLine($"Cache EXISTS: {key}");
            return false;
        }
    }
}
