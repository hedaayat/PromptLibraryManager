namespace PromptManager.Core.Models;

public sealed class Prompt
{
    public string Id { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();
    public string Template { get; init; } = string.Empty;
    public IReadOnlyList<PromptVariable> Variables { get; init; } = Array.Empty<PromptVariable>();
    public IReadOnlyList<Preset> Presets { get; init; } = Array.Empty<Preset>();
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
