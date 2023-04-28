using Contracts.Process;
using Sync.Client;

namespace App.Sync;
public class ProcessStartup : SyncStartup<ProcessSync, PackageContract>
{
    public ProcessStartup(
        IServiceProvider provider,
        IHostApplicationLifetime lifetime
    ) : base(provider, lifetime) { }
}

public static class SyncStartupExtensions
{
    public static void RegisterSyncClients(this IServiceCollection services)
    {
        services.AddSyncClient<ProcessSync, PackageContract>();
        services.AddHostedService<ProcessStartup>();
    }
}