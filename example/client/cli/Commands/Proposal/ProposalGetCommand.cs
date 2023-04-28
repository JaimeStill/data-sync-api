using System.CommandLine;
using Common;
using Common.Cli;
using Contracts.App;
using Contracts.Graph;
using SyncCli.Extensions;

namespace SyncCli.Commands;
public class ProposalGetCommand : CliCommand
{
    public ProposalGetCommand() : base(
        "get",
        "Get a Proposal via the App Graph API",
        new Func<string, int, Task>(Call),
        new()
        {
            new Option<int>(
                new string[] { "--id", "-i" },
                getDefaultValue: () => 0,
                description: "Proposal ID"
            )
        }
    ) { }

    static async Task Call(string graph, int id)
    {
        if (id < 0)
        {
            Console.WriteLine("An ID must be provided to get a Proposal");
            return;
        }

        AppGraph app = App.GetAppGraph(graph);

        Console.WriteLine($"Getting Proposal record {id}");
        ProposalContract? proposal = await app.GetProposal(id);

        if (proposal is null)
        {
            Console.WriteLine($"Proposal record {id} was not found");
            return;
        }

        App.PrintProposal(proposal);
    }
}