using PromptManager.Cli.Commands;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PromptManager.Cli;

public sealed class SettingsShowCommand : Command
{
    public override int Execute(Spectre.Console.Cli.CommandContext context)
    {
        var state = CommandUtilities.LoadContext();
        var table = new Table().RoundedBorder();
        table.AddColumn("Setting");
        table.AddColumn("Value");

        table.AddRow("storage.mode", state.Config.Storage.Mode);
        table.AddRow("storage.repoPath", state.Config.Storage.RepoPath);
        table.AddRow("providers.openai.enabled", state.Config.Providers.OpenAi.Enabled.ToString());
        table.AddRow("providers.openai.defaultModel", state.Config.Providers.OpenAi.DefaultModel);
        table.AddRow("providers.ollama.enabled", state.Config.Providers.Ollama.Enabled.ToString());
        table.AddRow("providers.ollama.host", state.Config.Providers.Ollama.Host);
        table.AddRow("providers.ollama.defaultModel", state.Config.Providers.Ollama.DefaultModel);
        table.AddRow("runtime.privacyMode", state.Config.Runtime.PrivacyMode.ToString());
        table.AddRow("runtime.storeRunHistory", state.Config.Runtime.StoreRunHistory.ToString());
        table.AddRow("runtime.outputRetentionDays", state.Config.Runtime.OutputRetentionDays.ToString());

        AnsiConsole.Write(table);
        return 0;
    }
}
