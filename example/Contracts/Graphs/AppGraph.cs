using Common.Graph;

namespace Contracts.Graph;
public class AppGraph : GraphClient
{
    public AppGraph(GraphService graph)
        : base(graph, "App") { }

    public async Task<List<ProposalContract>?> GetProposals() =>
        await Get<List<ProposalContract>?>("getProposals");

    public async Task<ProposalContract?> GetProposal(int id) =>
        await Get<ProposalContract?>($"getProposal/{id}");

    public async Task<ProposalContract?> SaveProposal(ProposalContract proposal) =>
        await Post<ProposalContract?, ProposalContract>(proposal, "saveProposal");

    public async Task<int?> RemoveProposal(ProposalContract proposal) =>
        await Delete<int?, ProposalContract>(proposal, "removeProposal");
}