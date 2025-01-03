using UrlShortner.Server.Interfaces;
using UrlShortner.Server.Models;

namespace UrlShortner.Server.Services
{
    public class SolrSearchService : ISearchService
    {
        // TODO: Inject ISolrOperations in constructor
        
        public async Task<string> GetLongUrlAsync(string shortCode)
        {
            // Placeholder: Implement Solr query
            Console.WriteLine($"Solr GET: {shortCode}");
            return null;
        }

        public async Task<bool> SaveUrlMappingAsync(string shortCode, string longUrl)
        {
            // Placeholder: Implement Solr add document
            Console.WriteLine($"Solr ADD: {shortCode} -> {longUrl}");
            return true;
        }

        public async Task IncrementClickCountAsync(string shortCode)
        {
            // Placeholder: Implement Solr atomic update
            Console.WriteLine($"Solr INCREMENT: {shortCode}");
        }

        public async Task<IEnumerable<string>> SearchUrlsAsync(string searchTerm)
        {
            // Placeholder: Implement Solr search
            Console.WriteLine($"Solr SEARCH: {searchTerm}");
            return new List<string>();
        }
    }
}
