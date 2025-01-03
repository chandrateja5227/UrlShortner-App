using Microsoft.AspNetCore.Mvc;
using UrlShortner.Server.Services;

namespace UrlShortner.Server.Controllers
{
    [ApiController]
    [Route("st")]
    public class UrlController : ControllerBase
    {
        private readonly UrlService _urlService;

        public UrlController(UrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost("shorten")]
        public async Task<ActionResult<string>> ShortenUrl([FromBody] string longUrl)
        {
            if (!Uri.TryCreate(longUrl, UriKind.Absolute, out _))
            {
                return BadRequest("Invalid URL format");
            }

            var shortCode = await _urlService.CreateShortUrlAsync(longUrl);
            var shortUrl = $"{Request.Scheme}://{Request.Host}/st/{shortCode}";
            
            return Ok(shortUrl);
        }

        [HttpGet("{shortCode}")]
        public async Task<ActionResult> RedirectToLongUrl(string shortCode)
        {
            var longUrl = await _urlService.GetLongUrlAsync(shortCode);
            
            if (longUrl == null)
            {
                return NotFound();
            }

            return Redirect(longUrl);
        }
    }
}
