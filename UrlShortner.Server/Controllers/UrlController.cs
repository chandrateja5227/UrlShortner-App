using Microsoft.AspNetCore.Mvc;
using UrlShortner.Server.Services;

namespace UrlShortner.Server.Controllers
{
    [ApiController]
    [Route("st")]
    public class UrlController : ControllerBase
    {
        private readonly UrlService _urlService;
        private readonly IConfiguration _configuration;

        public UrlController(UrlService urlService, IConfiguration configuration)
        {
            _urlService = urlService;
            _configuration = configuration;
        }

        [HttpPost("shorten")]
        public async Task<ActionResult<string>> ShortenUrl([FromBody] string longUrl)
        {
            if (!Uri.TryCreate(longUrl, UriKind.Absolute, out _))
            {
                return BadRequest("Invalid URL format");
            }

            var shortCode = await _urlService.CreateShortUrlAsync(longUrl);
            
            // Get the frontend URL from configuration
            var frontendUrl = _configuration["ClientApp:BaseUrl"] ?? "http://localhost:2510";
            var shortUrl = $"{frontendUrl}/st/{shortCode}";
            
            return Ok(shortUrl);
        }

        [HttpGet("{shortCode}")]
        public async Task<ActionResult> RedirectToLongUrl(string shortCode)
        {
            var longUrl = await _urlService.GetLongUrlAsync(shortCode);
            
            if (longUrl == null)
            {
                var clientUrl = _configuration["ClientApp:BaseUrl"] ?? "http://localhost:2510";
                return Redirect($"{clientUrl}/not-found");
            }

            return Redirect(longUrl);
        }
    }
}
