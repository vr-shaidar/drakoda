using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Drakoda.AI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Postgres") ?? throw new InvalidOperationException("Postgres connection string is required"), name: "postgres")
    .AddRedis(builder.Configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException("Redis connection string is required"), name: "redis");
builder.Services.AddDbContext<DrakodaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379"));
builder.Services.AddSingleton<IAIProviderAdapter, UnconfiguredProviderAdapter>();
builder.Services.AddSingleton<IProviderRouter, ProviderRouter>();
builder.Services.AddProblemDetails();
builder.Services.AddCors(options => options.AddPolicy("web", policy =>
    policy.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? ["http://localhost:3000"])
          .AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

var app = builder.Build();
app.UseExceptionHandler();
app.UseCors("web");
app.MapHealthChecks("/healthz");
app.MapHealthChecks("/healthz/ready");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();

public partial class Program { }
