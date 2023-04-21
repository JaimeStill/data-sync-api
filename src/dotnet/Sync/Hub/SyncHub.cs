using Microsoft.AspNetCore.SignalR;

namespace Sync.Hub;
public abstract class SyncHub : Hub<ISyncHub>
{
    public List<Channel> Channels { get; private set; }

    public SyncHub()
    {
        Channels = new();
    }

    async Task AddToChannel(string name, string connectionId, IGroupManager groups)
    {
        Channel? channel = Channels.FirstOrDefault(x => x.Name == name);

        if (channel is null)
            Channels.Add(
                new(
                    name,
                    new() { connectionId }
                )
            );
        else
            if (!channel.Connections.Contains(connectionId))
            channel.Connections.Add(connectionId);

        await groups.AddToGroupAsync(connectionId, name);
    }

    async Task RemoveFromChannel(string name, string connectionId, IGroupManager groups)
    {
        Channel? channel = Channels.FirstOrDefault(x => x.Name == name);

        if (channel is not null)
        {
            if (channel.Connections.Contains(connectionId))
                channel.Connections.Remove(connectionId);

            if (channel.Connections.Count < 1)
                Channels.Remove(channel);
        }

        await groups.RemoveFromGroupAsync(connectionId, name);
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        List<string> channels = Channels
            .Where(x => x.Connections.Contains(Context.ConnectionId))
            .Select(x => x.Name)
            .Distinct()
            .ToList();

        foreach (string channel in channels)
            await RemoveFromChannel(channel, Context.ConnectionId, Groups);

        await base.OnDisconnectedAsync(ex);
    }

    public async Task Join(string name)
    {
        Console.WriteLine($"Client {Context.ConnectionId} is joining channel {name}");
        await AddToChannel(name, Context.ConnectionId, Groups);
    }

    public async Task Leave(string name)
    {
        Console.WriteLine($"Client {Context.ConnectionId} is leaving channel {name}");
        await RemoveFromChannel(name, Context.ConnectionId, Groups);
    }

    static void LogAction<T>(ISyncMessage<T> message)
    {
        Console.WriteLine($"{message.Action} message received: {message.Message}");
        Console.WriteLine($"Message channel: {message.Channel}");
    }

    public async Task SendCreate<T>(ISyncMessage<T> message)
    {
        message.Action = ActionType.Create;
        
        LogAction(message);

        await Clients
            .OthersInGroup(message.Channel)
            .Create(message);
    }

    public async Task SendUpdate<T>(ISyncMessage<T> message)
    {
        message.Action = ActionType.Update;

        LogAction(message);

        await Clients
            .OthersInGroup(message.Channel)
            .Create(message);
    }

    public async Task SendSync<T>(ISyncMessage<T> message)
    {
        message.Action = ActionType.Sync;

        LogAction(message);

        await Clients
            .OthersInGroup(message.Channel)
            .Sync(message);
    }

    public async Task SendDelete<T>(ISyncMessage<T> message)
    {
        message.Action = ActionType.Delete;

        LogAction(message);

        await Clients
            .OthersInGroup(message.Channel)
            .Create(message);
    }
}