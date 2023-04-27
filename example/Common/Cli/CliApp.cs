using System.CommandLine;

namespace Common.Cli;
public class CliApp
{
    readonly RootCommand root;

    public CliApp(string name, List<CliCommand> commands, List<Option>? globals = null)
    {
        root = new(name);

        if (globals is not null && globals.Count > 0)
            globals.ForEach(root.AddGlobalOption);

        commands.ForEach(x => root.AddCommand(x.Build()));
    }

    public Task InvokeAsync(params string[] args) =>
        root.InvokeAsync(args);
}