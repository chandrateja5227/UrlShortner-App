using UrlShortner.Server.Interfaces;
using UrlShortner.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Register services
builder.Services.AddSingleton<ICacheService, RedisCacheService>();
builder.Services.AddSingleton<ISearchService, SolrSearchService>();
builder.Services.AddScoped<UrlService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Enable CORS
app.UseCors();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => "Healthy");

app.Run();
