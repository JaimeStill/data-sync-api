namespace Sync.Hub;
public abstract class ApiSyncHub<T> : SyncHub<T, IApiSyncHub<T>>
{
    public async Task SendAdd(ISyncMessage<T> message)
    {
        LogAction(message, "Add");

        await Clients
            .Others
            .Add(message);
    }

    public async Task SendUpdate(ISyncMessage<T> message)
    {
        LogAction(message, "Update");

        await Clients
            .Others
            .Update(message);
    }

    public async Task SendRemove(ISyncMessage<T> message)
    {
        LogAction(message, "Remove");

        await Clients
            .Others
            .Remove(message);
    }
}