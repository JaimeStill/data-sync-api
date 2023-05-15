using Common.Sync.Client;
using Microsoft.Extensions.Configuration;
using Sync;

namespace Contracts.Process;
public class PackageSync : ProcessSyncClient<PackageContract>
{
    static void WriteMessage(SyncMessage<PackageContract> message) =>
        Console.WriteLine(message.Message);

    void Initialize()
    {
        OnComplete.Set<SyncMessage<PackageContract>>(WriteMessage);
        OnReceive.Set<SyncMessage<PackageContract>>(WriteMessage);
        OnReject.Set<SyncMessage<PackageContract>>(WriteMessage);
        OnReturn.Set<SyncMessage<PackageContract>>(WriteMessage);
        OnSync.Set<SyncMessage<PackageContract>>(WriteMessage);
        OnWithdraw.Set<SyncMessage<PackageContract>>(WriteMessage);
    }

    public PackageSync(IConfiguration config) : base(
        config.GetValue<string>("Sync:Package") ?? "http://localhost:5002/sync/package"
    )
    {
        Initialize();
    }

    public PackageSync(string endpoint) : base(endpoint)
    {
        Initialize();
    }
}