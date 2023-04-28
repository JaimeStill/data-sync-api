using Common.Cli;
using Contracts;
using Contracts.Graph;
using SyncCli.Extensions;

namespace SyncCli.Commands;
public class ProposalListCommand : CliCommand
{
    public ProposalListCommand() : base(
        "list",
        "Get and list Proposal records via the App Graph API",
        new Func<string, Task>(Call)
    ) { }

    static async Task Call(string graph)
    {
        AppGraph app = App.GetAppGraph(graph);

        Console.WriteLine("Retrieving Proposals");

        List<ProposalContract>? result = await app.GetProposals();

        Console.WriteLine(result is null ? "No Proposals Found" : "Proposals:");
        result?.ForEach(App.PrintProposal);
    }
}