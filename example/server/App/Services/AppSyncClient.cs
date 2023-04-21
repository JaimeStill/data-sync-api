using Sync.Client;

namespace App.Services;
public class AppSyncClient : SyncClient
{
    public AppSyncClient(IConfiguration config) : base(
        config.GetValue<string>("Sync:App") ?? "http://localhost:5001/sync"
    ) { }
}