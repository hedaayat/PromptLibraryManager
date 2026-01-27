using PromptManager.Cli.Commands;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PromptManager.Cli;

public sealed class PromptListCommand : Command
{
    public override int Execute(Spectre.Console.Cli.CommandContext context)
    {
        var state = CommandUtilities.LoadContext();
        var repoPath = CommandUtilities.ResolveRepoPath(state.Config);
        var prompts = state.PromptRepository.ListPrompts(repoPath);

        if (prompts.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No prompts found.[/]");
            return 0;
        }

        var table = new Table().RoundedBorder();
        table.AddColumn("Slug");
        table.AddColumn("Title");
        table.AddColumn("Description");

        foreach (var prompt in prompts.OrderBy(p => p.Slug))
        {
            table.AddRow(prompt.Slug, prompt.Title, prompt.Description);
        }

        AnsiConsole.Write(table);
        return 0;
    }
}
