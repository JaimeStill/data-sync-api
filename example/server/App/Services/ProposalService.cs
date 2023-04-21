using App.Data;
using App.Hubs;
using App.Models;
using Common.Schema;
using Common.Services;
using Microsoft.AspNetCore.SignalR;
using Sync.Hub;

namespace App.Services;
public class ProposalService : SyncService<Proposal, AppSyncHub, AppDbContext>
{
    public ProposalService(AppDbContext db, IHubContext<AppSyncHub, ISyncHub<IContract>> sync) : base(db, sync) { }
}