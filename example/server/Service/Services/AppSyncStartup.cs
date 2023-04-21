using Common.Schema;
using Sync.Client;

namespace Service.Services;
public class AppSyncStartup : SyncStartup<AppSyncClient, IContract>
{
    public AppSyncStartup(IServiceProvider provider, IHostApplicationLifetime lifetime) : base(provider, lifetime) { }
}