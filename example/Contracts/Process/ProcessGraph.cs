using Common;
using Common.Graph;
using Contracts.Process;

namespace Contracts.Process;
public class ProcessGraph : GraphClient
{
    public ProcessGraph(GraphService graph)
        : base(graph, "Process") { }

    public async Task<List<PackageContract>?> GetAll() =>
        await Get<List<PackageContract>?>("getAll");

    public async Task<List<PackageContract>?> GetAllByState(ProcessState state) =>
        await Get<List<PackageContract>?>($"getAllByState/{state}");

    public async Task<PackageContract?> GetById(int id) =>
        await Get<PackageContract?>($"getById/{id}");

    public async Task<PackageContract?> GetByResource(int resourceId, string type) =>
        await Get<PackageContract?>($"getByResource/{resourceId}/{type}");

    public async Task<ApiResult<PackageContract>?> Complete(PackageContract package) =>
        await Post<ApiResult<PackageContract>?, PackageContract>(package, "complete");

    public async Task<ApiResult<PackageContract>?> Send(PackageContract package) =>
        await Post<ApiResult<PackageContract>?, PackageContract>(package, "receive");

    public async Task<ApiResult<PackageContract>?> Reject(PackageContract package, string status) =>
        await Post<ApiResult<PackageContract>?, PackageContract>(package, $"reject/{status}");

    public async Task<ApiResult<PackageContract>?> Return(PackageContract package, string status) =>
        await Post<ApiResult<PackageContract>?, PackageContract>(package, $"return/{status}");

    public async Task<ApiResult<PackageContract>?> Sync(PackageContract package, string status) =>
        await Post<ApiResult<PackageContract>?, PackageContract>(package, $"sync/{status}");

    public async Task<ApiResult<PackageContract>?> Withdraw(PackageContract package) =>
        await Delete<ApiResult<PackageContract>?, PackageContract>(package, "withdraw");
}