namespace PromptManager.Core.Models;

public sealed class RunHistoryItem
{
    public string PromptSlug { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public string Provider { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public IReadOnlyDictionary<string, string> Variables { get; init; } =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    public string RenderedPrompt { get; init; } = string.Empty;
    public string? Output { get; init; }
}
