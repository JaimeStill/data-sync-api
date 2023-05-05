using System.CommandLine;
using Common.Cli;
using Contracts.Graph;
using Contracts.Process;
using SyncCli.Extensions;

namespace SyncCli.Commands;
public class ProcessGetCommand : CliCommand
{
    public ProcessGetCommand() : base(
        "get",
        "Get and list a Package record by ResourceId and Type via the Process Service Graph API",
        new Func<string, int?, string?, Task>(Call),
        new()
        {
            new Option<int?>(
                new string[] { "--id", "-i" },
                description: "Package ID (if type is not provided). ID for the resource referenced by a Resource (if type is provided)."
            ),
            new Option<string?>(
                new string[] { "--type", "-t" },
                description: "Type of the resource referenced by a Resource."
            )
        }
    ) { }

    static async Task Call(string process, int? id, string? type)
    {
        if (id is null)
        {
            Console.WriteLine("An ID must be provided to retrieve a package");
            return;
        }

        ProcessGraph pg = App.GetProcessGraph(process);

        PackageContract? package;
        
        if (string.IsNullOrWhiteSpace(type))
        {
            Console.WriteLine($"Retrieving Package with Resource metadata: ID={id}");
            package = await pg.GetById(id.Value);
        }
        else
        {
            Console.WriteLine($"Retrieving Package with Resource metadata: Type={type}, ResourceId={id}");
            package = await pg.GetByResource(id.Value, type);
        }

        if (package is null)
        {
            Console.WriteLine("No Package was found");
            return;
        }

        App.PrintPackage(package);
    }
}