using Common.Schema;
using Service.Services;
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

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSyncClient<AppSyncClient, IContract>();
builder.Services.AddHostedService<AppSyncStartup>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
