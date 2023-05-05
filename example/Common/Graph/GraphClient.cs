using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Graph;
public abstract class GraphClient
{
    protected HttpClient http = new();
    protected Endpoint endpoint;
    protected Guid? endpointId;

    protected bool Available => endpointId.HasValue;

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

    public GraphClient(GraphService graph, string name)
    {
        endpoint = graph.GetEndpoint(name);
    }

    public async Task<Guid?> Initialize()
    {
        endpointId = await http.GetFromJsonAsync<Guid>(endpoint.Url);
        return endpointId;
    }

    protected async Task<Return?> Get<Return>(string method) =>
        await http.GetFromJsonAsync<Return?>(
            Path.Join(endpoint.Url, method),
            JsonOptions()
        );

    protected async Task<Return?> Post<Return, Data>(Data data, string method)
    {
        Return? result = await ReadResult<Return?>(
            await http.PostAsJsonAsync(
                Path.Join(endpoint.Url, method),
                data,
                JsonOptions()
            )
        );

        return result;
    }

    protected async Task<Return?> Delete<Return, Data>(Data data, string method) =>
        await ReadResult<Return?>(
            await http.SendAsync(
                new()
                {
                    Content = JsonContent.Create(data, options: JsonOptions()),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(
                        Path.Join(endpoint.Url, method)
                    )
                }
            )
        );

    protected static async Task<Return?> ReadResult<Return>(HttpResponseMessage response)
    {
        try
        {
            Return? result = await response
                .Content
                .ReadFromJsonAsync<Return>(JsonOptions());

            return result ?? default;
        }
        catch
        {
            return default;
        }
    }
}

public static class GraphClientExtensions
{
    public static void AddGraphClient<T>(this IServiceCollection services)
    where T : GraphClient =>
        services.AddScoped<T>();
}