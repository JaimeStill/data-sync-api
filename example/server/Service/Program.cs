using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Graph;
using Common.Middleware;
using Contracts.Graph;
using Service.Sync;

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
builder.Services.RegisterSyncClients();
builder.Services.AddGraphService();
builder.Services.AddGraphClient<AppGraph>();

var app = builder.Build();

app.UseJsonExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors();
app.MapControllers();

app.Run();
