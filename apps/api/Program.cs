using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Drakoda.AI;
using Drakoda.Api.Domain.Generations;
using Drakoda.Api.Domain.Pricing;
using Drakoda.Api.Domain.Storage;
using Drakoda.Api.Infrastructure.Queue;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Postgres") ?? throw new InvalidOperationException("Postgres connection string is required"), name: "postgres")
    .AddRedis(builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379", name: "redis");
builder.Services.AddDbContext<DrakodaDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379"));
builder.Services.AddSingleton<IAIProviderAdapter, UnconfiguredProviderAdapter>();
builder.Services.AddSingleton<IProviderRouter, ProviderRouter>();
builder.Services.AddScoped<AIModelRegistry>();
builder.Services.AddScoped<IAIGateway, AIGateway>();
builder.Services.AddSingleton<IGenerationQueue, RedisGenerationQueue>();
builder.Services.AddScoped<IGenerationJobService, GenerationJobService>();
builder.Services.AddHostedService<GenerationWorker>();
builder.Services.AddScoped<IPricingEngine, PricingEngine>();
builder.Services.AddSingleton<IObjectStorage, LocalObjectStorage>();
builder.Services.AddScoped<AssetService>();
builder.Services.AddProblemDetails();
builder.Services.AddCors(options => options.AddPolicy("web", policy => policy.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? ["http://localhost:3000"]).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

var app = builder.Build();
app.UseExceptionHandler();
app.UseCors("web");
app.MapHealthChecks("/healthz");
app.MapHealthChecks("/healthz/ready");
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.MapControllers();
app.Run();

public partial class Program { }
