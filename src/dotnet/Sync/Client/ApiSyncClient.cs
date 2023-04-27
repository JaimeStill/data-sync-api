using System.Diagnostics.CodeAnalysis;

namespace Sync.Client;
public abstract class ApiSyncClient<T> : SyncClient<T>, IApiSyncClient<T>
{
    public SyncAction OnAdd { get; protected set; }
    public SyncAction OnUpdate { get; protected set; }
    public SyncAction OnRemove { get; protected set; }

    public ApiSyncClient(string endpoint) : base(endpoint)
    {
        InitializeActions();
    }

    [MemberNotNull(
        nameof(OnAdd),
        nameof(OnUpdate),
        nameof(OnRemove)
    )]
    protected override void InitializeActions()
    {
        base.InitializeActions();
        OnAdd = new("Add", connection);
        OnUpdate = new("Update", connection);
        OnRemove = new("Remove", connection);
    }

    protected override void DisposeEvents()
    {
        base.DisposeEvents();
        OnAdd.Dispose();
        OnUpdate.Dispose();
        OnRemove.Dispose();
    }
}