using PromptManager.Cli.Commands;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PromptManager.Cli;

public sealed class PromptShowCommand : Command<PromptShowCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<slug>")]
        public string Slug { get; init; } = string.Empty;
    }

    public override int Execute(Spectre.Console.Cli.CommandContext context, Settings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Slug))
        {
            AnsiConsole.MarkupLine("[red]Slug is required.[/]");
            return -1;
        }

        var state = CommandUtilities.LoadContext();
        var repoPath = CommandUtilities.ResolveRepoPath(state.Config);
        var prompt = state.PromptRepository.GetPrompt(repoPath, settings.Slug);
        if (prompt == null)
        {
            AnsiConsole.MarkupLine($"[red]Prompt not found:[/] {settings.Slug}");
            return -1;
        }

        var panel = new Panel(prompt.Template)
        {
            Header = new PanelHeader($"{prompt.Title} ({prompt.Slug})"),
            Border = BoxBorder.Rounded
        };

        AnsiConsole.Write(panel);

        if (prompt.Variables.Count > 0)
        {
            var table = new Table().RoundedBorder();
            table.AddColumn("Key");
            table.AddColumn("Type");
            table.AddColumn("Required");
            table.AddColumn("Default");

            foreach (var variable in prompt.Variables)
            {
                table.AddRow(
                    variable.Key,
                    variable.Type,
                    variable.Required ? "yes" : "no",
                    variable.Default ?? string.Empty);
            }

            AnsiConsole.Write(table);
        }

        return 0;
    }
}
