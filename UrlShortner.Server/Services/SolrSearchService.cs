using System.Text.Json;
using UrlShortner.Server.Interfaces;

namespace UrlShortner.Server.Services
{
    public class SolrSearchService : ISearchService
    {
        private readonly HttpClient _httpClient;
        private readonly string _solrUrl;

        public SolrSearchService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _solrUrl = configuration["Solr:BaseUrl"] ?? "http://solr:8983/solr/urlshortener";
        }

        public async Task<string> GetLongUrlAsync(string shortCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_solrUrl}/select?q=short_code:{shortCode}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);
                var docs = doc.RootElement.GetProperty("response").GetProperty("docs");
                
                if (docs.GetArrayLength() > 0)
                {
                    return docs[0].GetProperty("long_url").GetString();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting long URL: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> SaveUrlMappingAsync(string shortCode, string longUrl)
        {
            try
            {
                var document = new
                {
                    id = shortCode,  // Use shortCode as ID
                    short_code = shortCode,
                    long_url = longUrl,
                    domain = new Uri(longUrl).Host,
                    created_date = DateTime.UtcNow.ToString("o"),
                    click_count = 0,
                    is_active = true
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(new[] { document }),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{_solrUrl}/update?commit=true", content);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving URL mapping: {ex.Message}");
                return false;
            }
        }

        public async Task IncrementClickCountAsync(string shortCode)
        {
            try
            {
                // Solr atomic update with shortCode as ID
                var update = new
                {
                    id = shortCode,
                    click_count = new { inc = 1 }
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(new[] { update }),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{_solrUrl}/update?commit=true", content);
                
                // Log the response for debugging
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Solr update response: {responseContent}");
                
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error incrementing click count: {ex.Message}");
            }
        }

        public async Task<IEnumerable<string>> SearchUrlsAsync(string searchTerm)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{_solrUrl}/select?q=long_url:{searchTerm}*&fl=long_url&rows=10"
                );
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);
                var docs = doc.RootElement.GetProperty("response").GetProperty("docs");
                
                var urls = new List<string>();
                foreach (var docElement in docs.EnumerateArray())
                {
                    if (docElement.TryGetProperty("long_url", out var longUrl))
                    {
                        urls.Add(longUrl.GetString());
                    }
                }

                return urls;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching URLs: {ex.Message}");
                return Enumerable.Empty<string>();
            }
        }
    }
}
