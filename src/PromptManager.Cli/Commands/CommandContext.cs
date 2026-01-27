using PromptManager.Core.Models;
using PromptManager.Infra.Yaml;

namespace PromptManager.Cli.Commands;

public sealed class CommandContext
{
    public CommandContext(string configPath, Config config, ConfigStore configStore, YamlPromptRepository promptRepository)
    {
        ConfigPath = configPath;
        Config = config;
        ConfigStore = configStore;
        PromptRepository = promptRepository;
    }

    public string ConfigPath { get; }
    public Config Config { get; }
    public ConfigStore ConfigStore { get; }
    public YamlPromptRepository PromptRepository { get; }
}
