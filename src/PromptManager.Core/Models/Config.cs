namespace PromptManager.Core.Models;

public sealed class Config
{
    public StorageConfig Storage { get; init; } = new();
    public ProviderConfig Providers { get; init; } = new();
    public RuntimeConfig Runtime { get; init; } = new();
}

public sealed class StorageConfig
{
    public string Mode { get; init; } = "yaml";
    public string RepoPath { get; init; } = "./prompt-library";
    public string DbPath { get; init; } = "./data/prompts.db";
}

public sealed class ProviderConfig
{
    public OpenAiConfig OpenAi { get; init; } = new();
    public OllamaConfig Ollama { get; init; } = new();
}

public sealed class OpenAiConfig
{
    public bool Enabled { get; init; } = true;
    public string ApiKeyEnv { get; init; } = "OPENAI_API_KEY";
    public string DefaultModel { get; init; } = "gpt-4.1-mini";
}

public sealed class OllamaConfig
{
    public bool Enabled { get; init; } = true;
    public string Host { get; init; } = "http://localhost:11434";
    public string DefaultModel { get; init; } = "llama3.1";
}

public sealed class RuntimeConfig
{
    public bool PrivacyMode { get; init; }
    public bool StoreRunHistory { get; init; } = true;
    public int OutputRetentionDays { get; init; }
}
