using System.Text.Json;
using StackExchange.Redis;
using UrlShortner.Server.Interfaces;

namespace UrlShortner.Server.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisCacheService(IConfiguration configuration)
        {
            var redisConnection = configuration.GetConnectionString("Redis") ?? "localhost:6379";
            _redis = ConnectionMultiplexer.Connect(redisConnection);
            _db = _redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var value = await _db.StringGetAsync(key);
                if (!value.HasValue)
                    return default;

                return JsonSerializer.Deserialize<T>(value!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis GET Error: {ex.Message}");
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            try
            {
                var jsonValue = JsonSerializer.Serialize(value);
                await _db.StringSetAsync(key, jsonValue, expiry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis SET Error: {ex.Message}");
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis DELETE Error: {ex.Message}");
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                return await _db.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis EXISTS Error: {ex.Message}");
                return false;
            }
        }
    }
}
