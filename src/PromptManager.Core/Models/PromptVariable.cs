namespace PromptManager.Core.Models;

public sealed class PromptVariable
{
    public string Key { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public string Type { get; init; } = "string";
    public bool Required { get; init; }
    public string? Default { get; init; }
    public IReadOnlyList<string> Options { get; init; } = Array.Empty<string>();
}
