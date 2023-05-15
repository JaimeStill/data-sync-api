using Common.Graph;
using Contracts.App;
using Contracts.Process;

namespace SyncCli.Extensions;
public static class App
{
    public static AppGraph GetAppGraph(string graph) =>
        new(GetGraphService("App", graph));

    public static ProcessGraph GetProcessGraph(string graph) =>
        new(GetGraphService("Process", graph));

    public static void PrintProposal(ProposalContract proposal) =>
        Console.WriteLine(proposal.ToString());

    public static void PrintPackage(PackageContract package) =>
        Console.WriteLine(package.ToString());

    public static void PrintResource(ResourceContract resource) =>
        Console.WriteLine(resource.ToString());

    static GraphService GetGraphService(string service, string graph) =>
        new(new Graph()
        {
            Id = Guid.NewGuid(),
            Endpoints = new()
            {
                new() { Name = service, Url = graph }
            }
        });
}