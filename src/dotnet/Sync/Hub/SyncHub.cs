using Microsoft.AspNetCore.SignalR;

namespace Sync.Hub;
public abstract class SyncHub<T, H> : Hub<H>
    where H : class, ISyncHub<T>
{
    protected static void LogAction(ISyncMessage<T> message, string action) =>
        Console.WriteLine($"{action} message received: {message.Message}");

    public async Task SendSync(ISyncMessage<T> message)
    {
        LogAction(message, "Sync");

        await Clients
            .Others
            .Sync(message);
    }
}