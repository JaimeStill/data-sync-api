using System.Text.Json;
using System.Text.Json.Serialization;
using App.Data;
using App.Hubs;
using App.Services;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Sync.Client;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddCors(options =>
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.WithExposedHeaders("Access-Control-Allow-Origin");
        })
    );

builder
    .Services
    .AddDbContext<AppDbContext>(options =>
    {
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        options.UseSqlServer(builder.Configuration.GetConnectionString("App"));
    })
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddAppServices();
builder.Services.AddSyncClient<AppSyncClient>();
builder.Services.AddHostedService<AppSyncStartup>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();
app.MapHubs();

app.Run();