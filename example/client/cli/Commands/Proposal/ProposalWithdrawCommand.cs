using System.CommandLine;
using Common;
using Common.Cli;
using Contracts.App;
using Contracts.Graph;
using Contracts.Process;
using SyncCli.Extensions;

namespace SyncCli.Commands;
public class ProposalWithdrawCommand : CliCommand
{
    public ProposalWithdrawCommand() : base(
        "withdraw",
        "Withdraw a Proposal Package via the Process Service Graph API",
        new Func<string, string, int?, Task>(Call),
        new()
        {
            new Option<int?>(
                new string[] { "--id", "-i" },
                description: "Proposal ID"
            )
        }
    ) { }

    static async Task Call(string app, string process, int? id)
    {
        if (id is null)
        {
            Console.WriteLine("An ID must be provided to withdraw a Proposal");
            return;
        }

        AppGraph ag = App.GetAppGraph(app);
        ProcessGraph pg = App.GetProcessGraph(process);

        Console.WriteLine($"Retrieving Proposal record {id}");
        ProposalContract? proposal = await ag.GetProposal(id.Value);

        if (proposal is null)
        {
            Console.WriteLine($"Proposal record {id} was not found");
            return;
        }

        Console.WriteLine($"Retrieving Package for Proposal {proposal.Name}");
        PackageContract? package = await pg.GetByResource(id.Value, proposal.Type);

        if (package is null)
        {
            Console.WriteLine($"No active Package was found for Proposal {proposal.Name}");
            return;
        }

        Console.WriteLine($"Withdrawing Package {package.Name}");
        ApiResult<PackageContract>? result = await pg.Withdraw(package);

        if (result is not null)
            Console.WriteLine(result.Message);
        else
            Console.WriteLine($"Proposal package {package.Name} was unable to be withdrawn");
    }
}