using Contracts;
using Sync;
using Sync.Client;

namespace Service.Sync;
public class ProposalClient : ApiSyncClient<ProposalContract>
{
    static void WriteMessage(SyncMessage<ProposalContract> message) =>
        Console.WriteLine(message.Message);

    public ProposalClient(IConfiguration config) : base(
        config.GetValue<string>("Sync:Proposal") ?? "http://localhost:5001/sync/proposal/"
    )
    {
        OnAdd.Set<SyncMessage<ProposalContract>>(WriteMessage);
        OnUpdate.Set<SyncMessage<ProposalContract>>(WriteMessage);
        OnSync.Set<SyncMessage<ProposalContract>>(WriteMessage);
        OnRemove.Set<SyncMessage<ProposalContract>>(WriteMessage);
    }
}