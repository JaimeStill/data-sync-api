using System.Text.Json;
using System.Text.Json.Serialization;
using App.Data;
using App.Hubs;
using App.Sync;
using Common.Graph;
using Common.Middleware;
using Common.Services;
using Contracts.Graph;
using Microsoft.EntityFrameworkCore;

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
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.RegisterSyncClients();
builder.Services.AddGraphService();
builder.Services.AddGraphClient<ProcessGraph>();
builder.Services.AddAppServices();

var app = builder.Build();

app.UseJsonExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.UseRouting();
app.UseCors();
app.MapControllers();
app.MapHubs();

app.Run();
