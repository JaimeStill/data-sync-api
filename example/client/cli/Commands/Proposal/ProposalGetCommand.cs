using System.CommandLine;
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
        new Func<string, int?, Task>(Call),
        new()
        {
            new Option<int?>(
                new string[] { "--id", "-i" },
                description: "Proposal ID"
            )
        }
    ) { }

    static async Task Call(string app, int? id)
    {
        if (id is null)
        {
            Console.WriteLine("An ID must be provided to get a Proposal");
            return;
        }

        AppGraph ag = App.GetAppGraph(app);

        Console.WriteLine($"Retrieving Proposal record {id}");
        ProposalContract? proposal = await ag.GetProposal(id.Value);

        if (proposal is null)
        {
            Console.WriteLine($"Proposal record {id} was not found");
            return;
        }

        App.PrintProposal(proposal);
    }
}