using App.Data;
using App.Models;
using Common.Services;

namespace App.Services;
public class ProposalService : SyncService<Proposal, AppDbContext>
{
    public ProposalService(AppDbContext db, AppSyncClient sync) : base(db, sync) { }
}