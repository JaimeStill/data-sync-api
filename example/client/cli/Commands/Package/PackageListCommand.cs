using System.CommandLine;
using Common.Cli;
using Contracts.Graph;
using Contracts.Process;
using SyncCli.Extensions;

namespace SyncCli.Commands;
public class PackageListCommand : CliCommand
{
    public PackageListCommand() : base(
        "list",
        "Get and list Process Package records via the Process Graph API",
        new Func<string, ProcessState?, Task>(Call),
        new()
        {
            new Option<ProcessState?>(
                new string[] { "--state", "-s" },
                getDefaultValue: () => null,
                description: "Package State"
            )
        }
    ) { }

    static async Task Call(string process, ProcessState? state)
    {
        ProcessGraph pg = App.GetProcessGraph(process);

        Console.WriteLine(
            state is null
                ? "Retrieving Packages"
                : $"Retrieving {state} Packages"
        );

        List<PackageContract>? result = state is null
            ? await pg.GetAll()
            : await pg.GetAllByState(state.Value);

        Console.WriteLine(
            result?.Count < 1
                ? "No Packages Found"
                : "Packages"
        );

        result?.ForEach(p =>
        {
            App.PrintPackage(p);

            if (p.Resources?.Count > 0)
                p.Resources
                    .ToList()
                    .ForEach(x => Console.WriteLine($"\t* {x}"));

            Console.WriteLine();
        });
    }
}