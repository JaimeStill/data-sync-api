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
        new Func<string, int, string, Task>(Call),
        new()
        {
            new Option<int>(
                new string[] { "--resourceId", "--id", "-i" },
                getDefaultValue: () => 0,
                description: "ID for the resource referenced by a Resource"
            ),
            new Option<string>(
                new string[] { "--type", "-t" },
                getDefaultValue: () => string.Empty,
                description: "Type of the resource referenced by a Resource"
            )
        }
    ) { }

    static async Task Call(string graph, int resourceId, string type)
    {
        if (resourceId < 1 || string.IsNullOrWhiteSpace(type))
        {
            Console.WriteLine("ResourceId and Type must be provided to get a Package");
            return;
        }

        ProcessGraph pg = App.GetProcessGraph(graph);

        Console.WriteLine($"Retrieving Package with Resource metadata: Type={type}, ResourceId={resourceId}");

        PackageContract? package = await pg.GetByResource(resourceId, type);

        if (package is null)
        {
            Console.WriteLine("No Package was found");
            return;
        }

        App.PrintPackage(package);
    }
}