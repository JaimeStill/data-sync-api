using Common;
using Common.Services;
using Common.Sync.Hub;
using Contracts.Process;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Process.Data;
using Process.Hubs;
using Process.Models;

namespace Process.Services;
public class PackageService : ProcessSyncService<Package, PackageHub, AppDbContext>
{
    public PackageService(
        AppDbContext db,
        IHubContext<PackageHub, IProcessSyncHub<Package>> sync
    ) : base(db, sync) { }

    public async Task<List<Package>> GetAllByState(ProcessState state) =>
        await query
            .Where(x => x.State == state)
            .OrderBy(x => x.Name)
            .ToListAsync();

    public async Task<Package?> GetByResource(int resourceId, string type)
    {
        Package? package = await query
            .FirstOrDefaultAsync(x =>
                x.Resources != null
                && x.Resources.Any(y =>
                    y.ResourceId == resourceId
                    && y.ResourceType == type
                )
            );

        return package;
    }

    protected override Func<Package, Task<Package>> OnComplete => (package) =>
    {
        package.Status = "Complete";
        package.State = ProcessState.Complete;

        return Task.FromResult(package);
    };

    protected override Func<Package, Task<Package>> OnReceive => (package) =>
    {
        package.Status = "Received";
        package.State = ProcessState.Pending;

        return Task.FromResult(package);
    };

    protected override Func<Package, string, Task<Package>> OnReject => (package, status) =>
    {
        package.Status = status;
        package.State = ProcessState.Rejected;

        return Task.FromResult(package);
    };

    protected override Func<Package, string, Task<Package>> OnReturn => (package, status) =>
    {
        package.Status = status;
        package.State = ProcessState.Returned;

        return Task.FromResult(package);
    };

    protected override Func<Package, string, Task<Package>> OnSync => (package, status) =>
    {
        package.Status = status;

        return Task.FromResult(package);
    };

    protected override IQueryable<Package> SetGraph(DbSet<Package> data) =>
        data.Include(x => x.Resources);

    protected override ValidationResult ValidateAction(Package package, string action)
    {
        ValidationResult result = new();

        if (package.State != ProcessState.Pending)
            result.AddMessage($"Unable to {action} a Package in a state other than Pending");

        return result;
    }

    protected override ValidationResult ValidateWithdrawal(Package package)
    {
        ValidationResult result = new();

        if (package.State != ProcessState.Pending || package.State != ProcessState.Returned)
            result.AddMessage("A Package can only be withdrawn if it is Pending or Returned");

        return result;
    }

    protected override async Task<ValidationResult> ValidateReceive(Package package)
    {
        ValidationResult result = new();

        if (package.Resources?.Count < 1)
            result.AddMessage("A Package must have at least one Resource associated");

        if (package.State != ProcessState.Pending && package.State != ProcessState.Returned)
            result.AddMessage("A Package may only be submitted if it is new or Returned");

        if (package.Resources is not null)
        {
            foreach (Resource resource in package.Resources)
            {
                bool exists = await db.Resources.AnyAsync(x =>
                    x.PackageId != package.Id
                    && x.Package != null
                    && (               
                        x.Package.State == ProcessState.Pending
                        || x.Package.State == ProcessState.Returned
                    )
                    && x.ResourceId == resource.ResourceId
                    && x.ResourceType == resource.ResourceType
                );

                if (exists)
                {
                    result.AddMessage("Package contains Resources already assigned to another Package");
                    break;
                }
            }
        }

        return result;
    }
}