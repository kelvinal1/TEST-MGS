using AspNetCoreRateLimit;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text;
using System.Globalization;
using CurbeCorporativo.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using AppControlProduct.DAL.Context;
using TestAppBack.Config;

const string _swaggerDocName = "v1.0";

var builder = WebApplication.CreateBuilder(args);

// Configure log.
// Add configuration Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(_swaggerDocName, new OpenApiInfo { Title = "API TEST APP v1.0", Version = "1.0" });

    options.CustomSchemaIds(type => type.FullName);
});

// PostgreSQL
builder.Services.AddDbContext<CorpContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresqlDBConnection"));
});

// Context
builder.Services.AddScoped<IDbContext, PgDbContext>();
builder.Services.AddHttpContextAccessor();

// Handlers
builder.Services.AddSingleton(new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
});

// Configuration Services
SevicesConfig.ConfigureServices(builder.Services);

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", policyBuilder =>
    {
        var listCords = new List<string>();
        builder.Configuration.GetSection("cors").Bind(listCords);

        // Apply the allowed origins from the configuration
        policyBuilder.WithOrigins(listCords.ToArray())
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
    });
});




builder.Services.AddControllers();

// Configuration for rate limit
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.ClientIdHeader = "X-ClientId";
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "1s",
            Limit = 25,
        }
    };
    options.QuotaExceededResponse = new QuotaExceededResponse
    {
        StatusCode = 429,
        ContentType = "application/json",
    };
});
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

// Set culture info
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/" + _swaggerDocName + "/swagger.json", "API CONTROL PRODUCTS v1.0");
    });
}

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("EnableCORS");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
