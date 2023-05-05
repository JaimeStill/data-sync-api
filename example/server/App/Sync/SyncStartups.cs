using Contracts.Process;
using Sync.Client;

namespace App.Sync;
public class PackageStartup : SyncStartup<PackageSync, PackageContract>
{
    public PackageStartup(
        IServiceProvider provider
    ) : base(provider) { }
}

public static class SyncStartupExtensions
{
    public static void RegisterSyncClients(this IServiceCollection services)
    {
        services.AddSyncClient<PackageSync, PackageContract>();
        services.AddHostedService<PackageStartup>();
    }
}