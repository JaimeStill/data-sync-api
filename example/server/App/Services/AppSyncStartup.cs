using Sync.Client;

namespace App.Services;
public class AppSyncStartup : SyncStartup<AppSyncClient>
{
    public AppSyncStartup(IServiceProvider provider, IHostApplicationLifetime lifetime) : base(provider, lifetime) { }
}