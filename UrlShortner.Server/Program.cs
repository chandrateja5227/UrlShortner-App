using UrlShortner.Server.Interfaces;
using UrlShortner.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services
builder.Services.AddSingleton<ICacheService, RedisCacheService>();
builder.Services.AddSingleton<ISearchService, SolrSearchService>();
builder.Services.AddScoped<UrlService>();

// TODO: Add Redis configuration
// builder.Services.AddStackExchangeRedisCache(options => {
//     options.Configuration = builder.Configuration.GetConnectionString("Redis");
// });

// TODO: Add Solr configuration
// builder.Services.AddSolrNet<UrlMapping>("http://localhost:8983/solr/urlshortener");

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
