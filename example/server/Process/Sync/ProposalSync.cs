using Common.Sync.Client;
using Contracts.App;
using Contracts.Process;
using Process.Data;
using Process.Models;
using Sync;

namespace Process.Sync;
public class ProposalSync : ApiSyncClient<ProposalContract>
{
    private readonly IServiceProvider provider;

    public ProposalSync(IConfiguration config, IServiceProvider provider) : base(
        config.GetValue<string>("Sync:Proposal") ?? "http://localhost:5001/sync/proposal"
    )
    {
        this.provider = provider;
        Initialize();
    }

    void Initialize()
    {
        OnAdd.Set<SyncMessage<ProposalContract>>(WriteMessage);
        OnUpdate.Set<SyncMessage<ProposalContract>>(WriteMessage);
        OnSync.Set<SyncMessage<ProposalContract>>(WriteMessage);
        OnRemove.Set<SyncMessage<ProposalContract>>(HandleOnRemove);
    }

    static void WriteMessage(SyncMessage<ProposalContract> message) =>
        Console.WriteLine(message.Message);

    Task HandleOnRemove(SyncMessage<ProposalContract> message) =>
        ExecuteServiceAction<AppDbContext>(provider, async db =>
        {
            WriteMessage(message);
            Console.WriteLine($"Checking for pending Packages associated with removed Proposal {message.Data.Name}");
            Console.WriteLine($"Data ID: {message.Data.Id} - Data Type: {message.Data.Type}");

            IQueryable<Package> packages = db
                .Packages
                .Where(x =>
                    (
                        x.State == ProcessState.Pending
                        || x.State == ProcessState.Returned
                    )
                    && x.Resources.Any(y =>
                        y.ResourceType == message.Data.Type
                        && y.ResourceId == message.Data.Id
                    )
                );

            if (packages.Any())
            {
                Console.WriteLine($"Removing {packages.Count()} associated Packages");
                db.Packages.RemoveRange(packages);
                await db.SaveChangesAsync();
                Console.WriteLine("Associated packages successfully removed");
            }
            else
                Console.WriteLine("No associated Packages were found");
        });
}