using Contracts.App;
using Microsoft.Extensions.Configuration;
using Sync;
using Sync.Client;

namespace Contracts.Proposal;
public class ProposalSync : ApiSyncClient<ProposalContract>
{
    static void WriteMessage(SyncMessage<ProposalContract> message) =>
        Console.WriteLine(message.Message);

    void Initialize()
    {        
        OnAdd.Set<SyncMessage<ProposalContract>>(WriteMessage);
        OnUpdate.Set<SyncMessage<ProposalContract>>(WriteMessage);
        OnSync.Set<SyncMessage<ProposalContract>>(WriteMessage);
        OnRemove.Set<SyncMessage<ProposalContract>>(WriteMessage);
    }

    public ProposalSync(IConfiguration config) : base(
        config.GetValue<string>("Sync:Proposal") ?? "http://localhost:5001/sync/proposal"
    )
    {
        Initialize();
    }

    public ProposalSync(string endpoint) : base(endpoint)
    {
        Initialize();
    }
}