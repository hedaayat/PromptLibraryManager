using PromptManager.Cli.Commands;
using PromptManager.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PromptManager.Cli;

public sealed class PromptRunCommand : Command<PromptRunCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "slug")]
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

        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var variable in prompt.Variables)
        {
            var label = string.IsNullOrWhiteSpace(variable.Label) ? variable.Key : variable.Label;
            var promptText = variable.Required ? $"{label} (required)" : label;
            var answer = AnsiConsole.Ask<string>(promptText, variable.Default ?? string.Empty);
            values[variable.Key] = answer;
        }

        var renderer = new TemplateRenderer();
        var missing = renderer.Validate(prompt, values);
        if (missing.Count > 0)
        {
            AnsiConsole.MarkupLine($"[red]Missing required values:[/] {string.Join(", ", missing)}");
            return -1;
        }

        var output = renderer.Render(prompt.Template, values);
        var panel = new Panel(output)
        {
            Header = new PanelHeader("Rendered Prompt"),
            Border = BoxBorder.Rounded
        };
        AnsiConsole.Write(panel);

        return 0;
    }
}
