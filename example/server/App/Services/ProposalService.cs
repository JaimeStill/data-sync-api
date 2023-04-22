using App.Data;
using App.Hubs;
using App.Models;
using Common.Services;
using Microsoft.AspNetCore.SignalR;
using Sync.Hub;

namespace App.Services;
public class ProposalService : SyncService<Proposal, ProposalHub, AppDbContext>
{
    public ProposalService(AppDbContext db, IHubContext<ProposalHub, ISyncHub<Proposal>> sync) : base(db, sync) { }
}