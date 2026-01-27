using PromptManager.Core.Models;
using PromptManager.Infra.Yaml;
using Spectre.Console;

namespace PromptManager.Cli.Commands;

public static class CommandUtilities
{
    public static CommandContext LoadContext()
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.yaml");
        var configStore = new ConfigStore();
        Config config;
        if (File.Exists(configPath))
        {
            config = configStore.Load(configPath);
        }
        else
        {
            config = configStore.CreateDefault(configPath);
            AnsiConsole.MarkupLine($"[green]Created default config at[/] {configPath}");
        }

        return new CommandContext(configPath, config, configStore, new YamlPromptRepository());
    }

    public static string ResolveRepoPath(Config config)
    {
        var repoPath = config.Storage.RepoPath;
        return Path.IsPathRooted(repoPath)
            ? repoPath
            : Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), repoPath));
    }
}
