namespace PromptManager.Core.Models;

public sealed class Preset
{
    public string Name { get; init; } = string.Empty;
    public IReadOnlyDictionary<string, string> Values { get; init; } =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
}
