using Common;
using Contracts.Process;
using Microsoft.EntityFrameworkCore;
using Process.Data;
using Process.Models;

namespace Process.Services;
public abstract class ProcessService
{
    protected AppDbContext db;
    protected IQueryable<Package> query;

    protected virtual Func<Package, Task<Package>>? OnComplete { get; set; }
    protected virtual Func<Package, Task<Package>>? OnReceive { get; set; }
    protected virtual Func<Package, Task<Package>>? OnReject { get; set; }
    protected virtual Func<Package, Task<Package>>? OnReturn { get; set; }
    protected virtual Func<Package, Task<Package>>? OnSync { get; set; }
    protected virtual Func<Package, Task<Package>>? OnWithdraw { get; set; }

    protected virtual Func<Package, Task>? AfterComplete { get; set; }
    protected virtual Func<Package, Task>? AfterReceive { get; set; }
    protected virtual Func<Package, Task>? AfterReject { get; set; }
    protected virtual Func<Package, Task>? AfterReturn { get; set; }
    protected virtual Func<Package, Task>? AfterSync { get; set; }
    protected virtual Func<Package, Task>? AfterWithdraw { get; set; }

    public ProcessService(AppDbContext db)
    {
        this.db = db;
        query = db.Packages.Include(x => x.Resources);
    }

    public async Task<List<Package>> GetAll() =>
        await query
            .OrderBy(x => x.Name)
            .ToListAsync();

    public async Task<List<Package>> GetAllByState(ProcessState state) =>
        await query
            .Where(x => x.State == state)
            .OrderBy(x => x.Name)
            .ToListAsync();

    public async Task<Package?> GetById(int id) =>
        await query
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Package?> GetByResource(int resourceId, string type) =>
        await query
            .FirstOrDefaultAsync(x =>
                x.Resources != null
                && x.Resources.Any(y =>
                    y.ResourceId == resourceId
                    && y.Type == type
                )
            );

    public async Task<ApiResult<Package>> Complete(Package package)
    {
        try
        {
            if (package.State != ProcessState.Pending)
                return new(new ValidationResult("Cannot Complete a Package in a state other than Pending"));

            db.Packages.Attach(package);
            package.Status = "Complete";
            package.State = ProcessState.Complete;

            if (OnComplete is not null)
                package = await OnComplete(package);

            await db.SaveChangesAsync();

            if (AfterComplete is not null)
                await AfterComplete(package);

            return new(package, $"Package {package.Name} completed");
        }
        catch (Exception ex)
        {
            return new("Complete", ex);
        }
    }

    public async Task<ApiResult<Package>> Receive(Package package)
    {
        try
        {
            if (package.Resources?.Count < 1)
                return new(new ValidationResult("A Package must have at least one Resource associated"));

            if (package.State != ProcessState.Pending || package.State != ProcessState.Returned)
                return new(new ValidationResult("A Package may only be submitted if it is new or Returned"));

            if (package.Id > 0)
                db.Packages.Attach(package);

            if (OnReceive is not null)
                package = await OnReceive(package);

            package.Status = "Received";

            if (package.Id < 1)
                await db.Packages.AddAsync(package);

            await db.SaveChangesAsync();

            if (AfterReceive is not null)
                await AfterReceive(package);

            return new(package, $"Package {package.Name} received");
        }
        catch (Exception ex)
        {
            return new("Receive", ex);
        }
    }

    public async Task<ApiResult<Package>> Reject(Package package, string status)
    {
        try
        {
            if (package.State != ProcessState.Pending)
                return new(new ValidationResult("Unable to Reject a Package in a state other than Pending"));

            db.Packages.Attach(package);
            package.Status = status;
            package.State = ProcessState.Rejected;

            if (OnReject is not null)
                package = await OnReject(package);

            await db.SaveChangesAsync();

            if (AfterReject is not null)
                await AfterReject(package);

            return new(package, $"Package {package.Name} rejected");
        }
        catch (Exception ex)
        {
            return new("Reject", ex);
        }
    }

    public async Task<ApiResult<Package>> Return(Package package, string status)
    {
        try
        {
            if (package.State != ProcessState.Pending)
                return new(new ValidationResult("Unable to Return a Package in a state other than Pending"));

            db.Packages.Attach(package);
            package.Status = status;
            package.State = ProcessState.Returned;

            if (OnReturn is not null)
                package = await OnReturn(package);

            await db.SaveChangesAsync();

            if (AfterReturn is not null)
                await AfterReturn(package);

            return new(package, $"Package {package.Name} returned");
        }
        catch (Exception ex)
        {
            return new("Return", ex);
        }
    }

    public async Task<ApiResult<Package>> Sync(Package package, string status)
    {
        try
        {
            if (package.State != ProcessState.Pending)
                return new(new ValidationResult("Unable to Sync a Package in a state other than Pending"));

            db.Packages.Attach(package);
            package.Status = status;

            if (OnSync is not null)
                package = await OnSync(package);

            await db.SaveChangesAsync();

            if (AfterSync is not null)
                await AfterSync(package);

            return new(package, $"Package {package.Name} synced with status {status}");
        }
        catch (Exception ex)
        {
            return new("Sync", ex);
        }
    }

    public async Task<ApiResult<Package>> Withdraw(Package package)
    {
        try
        {
            if (package.State != ProcessState.Pending || package.State != ProcessState.Returned)
                return new(new ValidationResult("A Package can only be Withdrawn if it is Pending or Returned"));

            if (OnWithdraw is not null)
                package = await OnWithdraw(package);

            db.Packages.Remove(package);
            await db.SaveChangesAsync();

            if (AfterWithdraw is not null)
                await AfterWithdraw(package);

            return new(package, $"Package {package.Name} withdrawn");
        }
        catch (Exception ex)
        {
            return new("", ex);
        }
    }
}