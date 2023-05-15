using Sync.Client;
using System.Diagnostics.CodeAnalysis;

namespace Common.Sync.Client;
public abstract class ApiSyncClient<T> : SyncClient<T>, IApiSyncClient<T>
{
    public SyncAction OnAdd { get; protected set; }
    public SyncAction OnUpdate { get; protected set; }
    public SyncAction OnRemove { get; protected set; }

    [MemberNotNull(
        nameof(OnAdd),
        nameof(OnUpdate),
        nameof(OnRemove)
    )]
    void Initialize()
    {
        OnAdd = new("Add", connection);
        OnUpdate = new("Update", connection);
        OnRemove = new("Remove", connection);
    }

    public ApiSyncClient(string endpoint) : base(endpoint)
    {
        Initialize();
    }

    protected override void DisposeEvents()
    {
        base.DisposeEvents();
        OnAdd.Dispose();
        OnUpdate.Dispose();
        OnRemove.Dispose();
    }
}