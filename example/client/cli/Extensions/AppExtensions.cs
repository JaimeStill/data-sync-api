using Common.Graph;
using Contracts;
using Contracts.Graph;

namespace SyncCli.Extensions;
public static class App
{
    public static AppGraph GetAppGraph(string graph) =>
        new(GetGraphService(graph));

    public static string ToString(ProposalContract proposal) =>
        $"{proposal.Id} - {proposal.Name} - {proposal.Description}";

    public static void PrintProposal(ProposalContract proposal) =>
        Console.WriteLine(ToString(proposal));

    static GraphService GetGraphService(string graph) =>
        new(new Graph()
        {
            Id = Guid.NewGuid(),
            Endpoints = new()
            {
                new() { Name = "App", Url = graph }
            }
        });
}