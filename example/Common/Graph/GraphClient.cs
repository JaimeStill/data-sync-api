using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Graph;
public abstract class GraphClient
{
    protected HttpClient http = new();
    protected Endpoint endpoint;
    protected Guid? endpointId;
    
    protected bool Available => endpointId.HasValue;

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
            Path.Join(endpoint.Url, method)
        );

    protected async Task<Return?> Post<Return,Data>(Data data, string method) =>
        await ReadResult<Return?>(
            await http.PostAsJsonAsync(
                Path.Join(endpoint.Url, method),
                data
            )
        );

    protected async Task<Return?> Delete<Return,Data>(Data data, string method) =>
        await ReadResult<Return?>(
            await http.SendAsync(
                new()
                {
                    Content = JsonContent.Create(data),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(
                        Path.Join(endpoint.Url, method)
                    )
                }
            )
        );

    protected static async Task<Return?> ReadResult<Return>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            Return? result = await response
                .Content
                .ReadFromJsonAsync<Return>();

            return result;
        }
        else
            return default;
    }
}

public static class GraphClientExtensions
{
    public static void AddGraphClient<T>(this IServiceCollection services)
    where T : GraphClient =>
        services.AddScoped<T>();
}