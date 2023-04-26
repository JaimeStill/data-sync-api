using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR.Client;

namespace Sync.Client;
public abstract class SyncClient<T> : ISyncClient<T>
{
    protected readonly HubConnection connection;
    protected readonly string endpoint;

    public SyncClientStatus Status => new(connection.ConnectionId, connection.State);

    public SyncAction OnSync { get; protected set; }

    public SyncClient(string endpoint)
    {
        this.endpoint = endpoint;

        Console.WriteLine($"Building Sync connection at {endpoint}");
        connection = BuildHubConnection(endpoint);

        InitializeEvents();
        InitializeActions();
    }

    public async Task Connect(CancellationToken token = new())
    {
        if (connection.State != HubConnectionState.Connected)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"Connecting to {endpoint}");
                    await connection.StartAsync(token);
                    Console.WriteLine($"Now listening on {endpoint}");
                    return;
                }
                catch when (token.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect to {endpoint}");
                    Console.WriteLine(ex.Message);
                    await Task.Delay(5000, token);
                }
            }
        }
    }

    protected virtual HubConnection BuildHubConnection(string endpoint) =>
        new HubConnectionBuilder()
            .WithUrl(endpoint)
            .WithAutomaticReconnect()
            .Build();

    protected virtual void InitializeEvents()
    {
        connection.Closed += async (error) =>
        {
            await Task.Delay(5000);
            await Connect();
        };
    }

    [MemberNotNull(
        nameof(OnSync)
    )]
    protected virtual void InitializeActions()
    {
        OnSync = new("Sync", connection);
    }

    public async ValueTask DisposeAsync()
    {
        DisposeEvents();
        await DisposeConnection()
            .ConfigureAwait(false);

        GC.SuppressFinalize(this);
    }

    protected virtual void DisposeEvents()
    {
        OnSync.Dispose();
    }

    protected virtual async ValueTask DisposeConnection()
    {
        if (connection is not null)
        {
            await connection
                .DisposeAsync()
                .ConfigureAwait(false);
        }
    }
}