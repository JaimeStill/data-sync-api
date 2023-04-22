using Contracts;
using Sync.Client;

namespace Service.Sync;
public class ProposalStartup : SyncStartup<ProposalClient, ProposalContract>
{
    public ProposalStartup(
        IServiceProvider provider,
        IHostApplicationLifetime lifetime
    ) : base(provider, lifetime) { }
}

public static class SyncStartupExtensions
{
    public static void RegisterSyncClients(this IServiceCollection services)
    {
        services.AddSyncClient<ProposalClient, ProposalContract>();
        services.AddHostedService<ProposalStartup>();
    }
}