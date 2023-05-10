# Lessons Learned

## JSON Serialization

It's incredibly important to ensure that every aspect of the JSON configuration pipeline is appropriately configured. There was a nasty bug in the `SyncClient` that was preventing received Web Socket messages from being properly received because of this. The following scenario outlines this issue and illustrates all of the places that JSON serialization options should be configured.

At a future point, a singularly configurable JSON serialization options setup should be established to feed into each configuration point. This would simplify the overall configuration and avoid these issues in the future.

In the scenario this issue was encountered, the [`PackageHub`](./example/server/Process/Hubs/PackageHub.cs) was broadcasting [`ISyncMesssage`](./src/dotnet/Sync/ISyncMessage.cs) with a [`Package`](./example/server/Process/Models/Package.cs) as the `Data` type. `Package` contains a property represented by the [`ProcessState`](./example/Contracts/Process/ProcessState.cs) enum. The Entity Framework [`PackageConfig`](./example/server/Process/Data/Config/PackageConfig.cs) is setup to store the value of `Package.State` as the string representation of `ProcessState` (i.e. - instead of being stored as `0`, it is stored as `Pending`).

To facilitate this translation in Web API and SignalR, I configured `.AddJsonOptions` and `.AddJsonProtocol` in the `Program.cs` of both the [`App`](./example/server/App/Program.cs) and [`Process`](./example/server/Process/Program.cs) server projects. What I failed to realize is that the SignalR client needs to configure JSON serialization options through `HubConnectionBuilder` as well. As a result, when I would execute actions that broadcast events from `PackageHub`, the client was quietly throwing exceptions because logging is not configured for a `HubConnection` unless explicitly configured. Once I enabled logging, I was able to see the following message that helped me understand why the Web Socket events were not being appropriately received by the client:

```
fail: Microsoft.AspNetCore.SignalR.Client.HubConnection[57]
      Failed to bind arguments received in invocation '(null)' of 'Sync'.
      System.IO.InvalidDataException: Error binding arguments. Make sure that the types of the provided values match the types of the hub method being invoked.
       ---> System.Text.Json.JsonException: The JSON value could not be converted to Contracts.Process.ProcessState. Path: $.data.state | LineNumber: 0 | BytePositionInLine: 152.
```

Once I configured the `JsonSerializerOptions` on the `HubConnectionBuilder`, everything started working as intended.

### Configuration Points

**Web API Serialization**  

This is configured in the `Program.cs` of an ASP.NET Core project:

```cs
builder
    .Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptinos.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
```

**Graph Client Serialization**

This is configured in [`GraphClient`](./example/Common/Graph/GraphClient.cs) and used in all of the integrated HTTP calls:

```cs
protected static JsonSerializerOptions JsonOptions()
{
    JsonSerializerOptions options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    options.Converters.Add(new JsonStringEnumConverter());

    return options;
}
```

**SignalR Server Serialization**

This is configured in the `Program.cs` of an ASP.NET Core project:

```cs
builder
    .Services
    .AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
```

**SignalR Client Serialization**

This is configured when initializing a `HubConnection` via the `HubConnectionBuilder`:

```cs
new HubConnectionBuilder()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .WithUrl(endpoint)
    .Build();
```

## Tracing SignalR Issues

Encountering the above issue, though frustrating, was really helpful for illustrating how to troubleshoot issues with SignalR. The following configuration points control how SignalR logs information:

**SignalR Server Logging**  

Configured in [`appsettings.json`](./example/server/App/appsettings.Development.json):

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.AspNetCore.SignalR": "Warning",
      "Microsoft.AspNetCore.Http.Connections": "Warning"
    }
  }
}
```

**SignalR Client Logging**

Configured when initializing a `HubConnection` via the `HubConnectionBuilder`:

```cs
new HubConnectionBuilder()
    .WithUrl(endpoint)
    .ConfigureLogging(logging => {
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Debug);
    })
    .Build();
```