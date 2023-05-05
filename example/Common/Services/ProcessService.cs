using Common.Schema;
using Microsoft.EntityFrameworkCore;

namespace Common.Services;
public abstract class ProcessService<T, Db>
    where T : Entity
    where Db : DbContext
{
    protected Db db;
    protected IQueryable<T> query;

    protected string Type => typeof(T).Name;

    protected virtual Func<T, Task<T>>? OnComplete { get; set; }
    protected virtual Func<T, Task<T>>? OnReceive { get; set; }
    protected virtual Func<T, string, Task<T>>? OnReject { get; set; }
    protected virtual Func<T, string, Task<T>>? OnReturn { get; set; }
    protected virtual Func<T, string, Task<T>>? OnSync { get; set; }
    protected virtual Func<T, Task<T>>? OnWithdraw { get; set; }

    protected virtual Func<T, Task>? AfterComplete { get; set; }
    protected virtual Func<T, Task>? AfterReceive { get; set; }
    protected virtual Func<T, Task>? AfterReject { get; set; }
    protected virtual Func<T, Task>? AfterReturn { get; set; }
    protected virtual Func<T, Task>? AfterSync { get; set; }
    protected virtual Func<T, Task>? AfterWithdraw { get; set; }

    public ProcessService(Db db)
    {
        this.db = db;
        query = SetGraph(db.Set<T>());
    }

    #region Internal

    protected virtual IQueryable<T> SetGraph(DbSet<T> data) =>
        data;

    protected abstract ValidationResult ValidateAction(T entity, string action);
    protected abstract Task<ValidationResult> ValidateReceive(T entity);
    protected abstract ValidationResult ValidateWithdrawal(T entity);

    #endregion

    #region Public

    public async Task<List<T>> GetAll() =>
        await query
            .OrderBy(x => x.Name)
            .ToListAsync();

    public async Task<T?> GetById(int id) =>
        await query
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ApiResult<T>> Complete(T entity)
    {
        try
        {
            ValidationResult validity = ValidateAction(entity, "complete");

            if (validity.IsValid)
            {

                db.Set<T>().Attach(entity);

                if (OnComplete is not null)
                    entity = await OnComplete(entity);

                await db.SaveChangesAsync();

                if (AfterComplete is not null)
                    await AfterComplete(entity);

                return new(entity, $"{Type} {entity.Name} completed");
            }
            else
                return new(validity);
        }
        catch (Exception ex)
        {
            return new("Complete", ex);
        }
    }

    public async Task<ApiResult<T>> Receive(T entity)
    {
        try
        {
            ValidationResult validity = await ValidateReceive(entity);

            if (validity.IsValid)
            {
                if (entity.Id > 0)
                    db.Set<T>().Attach(entity);

                if (OnReceive is not null)
                    entity = await OnReceive(entity);

                if (entity.Id < 1)
                    await db.Set<T>().AddAsync(entity);

                await db.SaveChangesAsync();

                if (AfterReceive is not null)
                    await AfterReceive(entity);

                return new(entity, $"{Type} {entity.Name} received");
            }
            else
                return new(validity);
        }
        catch (Exception ex)
        {
            return new("Receive", ex);
        }
    }

    public async Task<ApiResult<T>> Reject(T entity, string status)
    {
        try
        {
            ValidationResult validity = ValidateAction(entity, "reject");

            if (validity.IsValid)
            {
                db.Set<T>().Attach(entity);

                if (OnReject is not null)
                    entity = await OnReject(entity, status);

                await db.SaveChangesAsync();

                if (AfterReject is not null)
                    await AfterReject(entity);

                return new(entity, $"{Type} {entity.Name} rejected");
            }
            else
                return new(validity);
        }
        catch (Exception ex)
        {
            return new("Reject", ex);
        }
    }

    public async Task<ApiResult<T>> Return(T entity, string status)
    {
        try
        {
            ValidationResult validity = ValidateAction(entity, "return");

            if (validity.IsValid)
            {
                db.Set<T>().Attach(entity);

                if (OnReturn is not null)
                    entity = await OnReturn(entity, status);

                await db.SaveChangesAsync();

                if (AfterReturn is not null)
                    await AfterReturn(entity);

                return new(entity, $"{Type} {entity.Name} returned");
            }
            else
                return new(validity);
        }
        catch (Exception ex)
        {
            return new("Return", ex);
        }
    }

    public async Task<ApiResult<T>> Sync(T entity, string status)
    {
        try
        {
            ValidationResult validity = ValidateAction(entity, "sync");

            if (validity.IsValid)
            {
                db.Set<T>().Attach(entity);

                if (OnSync is not null)
                    entity = await OnSync(entity, status);

                await db.SaveChangesAsync();

                if (AfterSync is not null)
                    await AfterSync(entity);

                return new(entity, $"{Type} {entity.Name} synced with status {status}");
            }
            else
                return new(validity);
        }
        catch (Exception ex)
        {
            return new("Sync", ex);
        }
    }

    public async Task<ApiResult<T>> Withdraw(T entity)
    {
        try
        {            
            ValidationResult validity = ValidateWithdrawal(entity);

            if (validity.IsValid)
            {
                if (OnWithdraw is not null)
                    entity = await OnWithdraw(entity);

                db.Set<T>().Remove(entity);
                await db.SaveChangesAsync();

                if (AfterWithdraw is not null)
                    await AfterWithdraw(entity);

                return new(entity, $"{Type} {entity.Name} withdrawn");
            }
            else
                return new(validity);
        }
        catch (Exception ex)
        {
            return new("Withdraw", ex);
        }
    }

    #endregion
}