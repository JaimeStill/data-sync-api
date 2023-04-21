using Common.Schema;
using Sync;
using Sync.Client;

namespace Service.Services;
public class AppSyncClient : SyncClient<IContract>
{
    static void WriteMessage(SyncMessage<IContract> message) =>
        Console.WriteLine(message);

    public AppSyncClient(IConfiguration config) : base(
        config.GetValue<string>("Sync:App") ?? "http://localhost:5001/sync"
    )
    {
        OnAdd.Set<SyncMessage<IContract>>(WriteMessage);
        OnUpdate.Set<SyncMessage<IContract>>(WriteMessage);
        OnSync.Set<SyncMessage<IContract>>(WriteMessage);
        OnRemove.Set<SyncMessage<IContract>>(WriteMessage);
    }
}