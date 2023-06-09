using App.Data;
using App.Hubs;
using App.Models;
using Common.Services;
using Common.Sync.Hub;
using Microsoft.AspNetCore.SignalR;

namespace App.Services;
public class ProposalService : ApiSyncService<Proposal, ProposalHub, AppDbContext>
{
    public ProposalService(
        AppDbContext db,
        IHubContext<ProposalHub,
        IApiSyncHub<Proposal>> sync
    ) : base(db, sync) { }
}