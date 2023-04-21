using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR.Client;

namespace Sync.Client;
public abstract class SyncClient : ISyncClient
{
    protected readonly HubConnection connection;
    protected readonly string endpoint;
    
    protected List<string> Channels { get; set; }
    public SyncClientStatus Status => new(connection.ConnectionId, connection.State);

    public SyncAction OnAdd { get; protected set; }
    public SyncAction OnUpdate { get; protected set; }
    public SyncAction OnSync { get; protected set; }
    public SyncAction OnRemove { get; protected set; }

    public SyncClient(string endpoint)
    {
        this.endpoint = endpoint;

        Channels = new();

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

    public async Task Join(string name)
    {
        if (connection.State != HubConnectionState.Connected)
            await Connect();

        if (Channels.Contains(name))
            Console.WriteLine($"Already connected to channel {name}");
        else
        {
            Console.WriteLine($"Joining channel {name}");
            await connection.InvokeAsync("Join", name);
            Channels.Add(name);
            Console.WriteLine($"Channel {name} successfully joined");
        }
    }

    public async Task Leave(string name)
    {
        if (Channels.Contains(name))
        {
            Console.WriteLine($"Leaving channel {name}");
            await connection.InvokeAsync("Leave", name);
            Channels.Remove(name);
            Console.WriteLine($"Channel {name} successfully left");
        }
        else
            Console.WriteLine($"Not connected to channel {name}");
    }

    public async Task Add<T>(ISyncMessage<T> message) =>
        await connection.InvokeAsync("SendCreate", message);

    public async Task Update<T>(ISyncMessage<T> message) =>
        await connection.InvokeAsync("SendUpdate", message);

    public async Task Sync<T>(ISyncMessage<T> message) =>
        await connection.InvokeAsync("SendSync", message);

    public async Task Remove<T>(ISyncMessage<T> message) =>
        await connection.InvokeAsync("SendDelete", message);

    protected virtual HubConnection BuildHubConnection(string endpoint) =>
        new HubConnectionBuilder()
            .WithUrl(endpoint)
            .WithAutomaticReconnect()
            .Build();

    protected void InitializeEvents()
    {
        connection.Closed += async (error) =>
        {
            await Task.Delay(5000);
            await Connect();
        };
    }

    [MemberNotNull(
        nameof(OnAdd),
        nameof(OnUpdate),
        nameof(OnSync),
        nameof(OnRemove)
    )]
    protected void InitializeActions()
    {
        OnAdd = new("Add", connection);
        OnUpdate = new("Update", connection);
        OnSync = new("Sync", connection);
        OnRemove = new("Remove", connection);
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
        OnAdd.Dispose();
        OnUpdate.Dispose();
        OnSync.Dispose();
        OnRemove.Dispose();
    }

    protected virtual async ValueTask DisposeConnection()
    {
        if (connection is not null)
        {
            foreach (string channel in Channels)
                await Leave(channel);

            await connection
                .DisposeAsync()
                .ConfigureAwait(false);
        }
    }
}