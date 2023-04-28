using Common;
using Common.Graph;
using Contracts.App;

namespace Contracts.Graph;
public class AppGraph : GraphClient
{
    public AppGraph(GraphService graph)
        : base(graph, "App") { }

    public async Task<List<ProposalContract>?> GetProposals() =>
        await Get<List<ProposalContract>?>("getProposals");

    public async Task<ProposalContract?> GetProposal(int id) =>
        await Get<ProposalContract?>($"getProposal/{id}");

    public async Task<ApiResult<ProposalContract>?> SaveProposal(ProposalContract proposal) =>
        await Post<ApiResult<ProposalContract>?, ProposalContract>(proposal, "saveProposal");

    public async Task<ApiResult<int>?> RemoveProposal(ProposalContract proposal) =>
        await Delete<ApiResult<int>?, ProposalContract>(proposal, "removeProposal");
}