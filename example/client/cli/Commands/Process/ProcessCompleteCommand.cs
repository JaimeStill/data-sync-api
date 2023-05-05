using System.CommandLine;
using Common;
using Common.Cli;
using Contracts.Graph;
using Contracts.Process;
using SyncCli.Extensions;

namespace SyncCli.Commands;
public class ProcessCompleteCommand : CliCommand
{
    public ProcessCompleteCommand() : base(
        "complete",
        "Complete a Package via the Process Service Graph API",
        new Func<string, int?, Task>(Call),
        new()
        {
            new Option<int?>(
                new string[] { "--id", "-i" },
                description: "Package ID"
            )
        }
    ) { }

    static async Task Call(string process, int? id)
    {
        if (id is null)
        {
            Console.WriteLine("An ID must be provided to complete a Package");
            return;
        }

        ProcessGraph pg = App.GetProcessGraph(process);

        Console.WriteLine($"Retrieving Package record {id}");
        PackageContract? package = await pg.GetById(id.Value);

        if (package is null)
        {
            Console.WriteLine($"Package record {id} was not found");
            return;
        }

        Console.WriteLine($"Completing Package {package.Name}");

        ApiResult<PackageContract>? result = await pg.Complete(package);

        if (result is not null)
            Console.WriteLine(result.Message);
        else
            Console.WriteLine("Package was unable to be completed");
    }
}