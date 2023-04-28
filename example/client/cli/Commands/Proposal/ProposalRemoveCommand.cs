using System.CommandLine;
using Common;
using Common.Cli;
using Contracts.App;
using Contracts.Graph;
using SyncCli.Extensions;

namespace SyncCli.Commands;
public class ProposalRemoveCommand : CliCommand
{
    public ProposalRemoveCommand() : base(
        "remove",
        "Remove a Proposal via the App Graph API",
        new Func<string, int?, Task>(Call),
        new()
        {
            new Option<int?>(
                new string[] { "--id", "-i" },
                description: "Proposal ID"
            )
        }
    )
    { }

    static async Task Call(string graph, int? id)
    {
        if (id is null)
        {
            Console.WriteLine("An ID must be provided to remove a Proposal");
            return;
        }

        AppGraph app = App.GetAppGraph(graph);

        Console.WriteLine($"Checking if Proposal record {id} exists");
        ProposalContract? proposal = await app.GetProposal(id.Value);

        if (proposal is null)
        {
            Console.WriteLine($"Proposal record {id} was not found");
            return;
        }

        Console.WriteLine($"Removing Proposal: {proposal}");

        ApiResult<int>? result = await app.RemoveProposal(proposal);

        if (result is not null)
            Console.WriteLine(result.Message);
    }
}