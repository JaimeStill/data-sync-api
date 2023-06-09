using System.CommandLine;
using Common;
using Common.Cli;
using Contracts.Process;
using SyncCli.Extensions;

namespace SyncCli.Commands;
public class PackageRejectCommand : CliCommand
{
    public PackageRejectCommand() : base(
        "reject",
        "Reject a Package via the Process Service Graph API",
        new Func<string, int?, string?, Task>(Call),
        new()
        {
            new Option<int?>(
                new string[] { "--id", "-i" },
                description: "Package ID"
            ),
            new Option<string?>(
                new string[] { "--status", "-s" },
                description: "Package Status"
            )
        }
    ) { }

    static async Task Call(string process, int? id, string? status)
    {
        if (id is null)
        {
            Console.WriteLine("An ID must be provided to reject a Package");
            return;
        }

        if (string.IsNullOrWhiteSpace(status))
        {
            Console.WriteLine("A resulting Status must be provided to reject a Package");
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

        Console.WriteLine($"Rejecting Package: {package.Name}");

        ApiResult<PackageContract>? result = await pg.Reject(package, status);

        if (result is not null)
            Console.WriteLine(result.Message);
        else
            Console.WriteLine("Package was unable to be rejected");
    }
}