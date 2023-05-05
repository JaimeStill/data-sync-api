using Contracts.App;
using Contracts.Proposal;
using Sync.Client;

namespace Process.Sync;
public class ProposalStartup : SyncStartup<ProposalSync, ProposalContract>
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
        services.AddSyncClient<ProposalSync, ProposalContract>();
        services.AddHostedService<ProposalStartup>();
    }
}