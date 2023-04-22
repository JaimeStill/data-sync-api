using Microsoft.AspNetCore.SignalR;

namespace Sync.Hub;
public abstract class SyncHub<T> : Hub<ISyncHub<T>>
{
    static void LogAction(ISyncMessage<T> message)
    {
        Console.WriteLine($"{message.Action} message received: {message.Message}");
    }

    public async Task SendCreate(ISyncMessage<T> message)
    {
        message.Action = ActionType.Add;
        
        LogAction(message);

        await Clients
            .Others
            .Add(message);
    }

    public async Task SendUpdate(ISyncMessage<T> message)
    {
        message.Action = ActionType.Update;

        LogAction(message);

        await Clients
            .Others
            .Add(message);
    }

    public async Task SendSync(ISyncMessage<T> message)
    {
        message.Action = ActionType.Sync;

        LogAction(message);

        await Clients
            .Others
            .Sync(message);
    }

    public async Task SendDelete(ISyncMessage<T> message)
    {
        message.Action = ActionType.Remove;

        LogAction(message);

        await Clients
            .Others
            .Add(message);
    }
}