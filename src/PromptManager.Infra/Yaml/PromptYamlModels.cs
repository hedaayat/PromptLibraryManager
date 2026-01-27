namespace PromptManager.Infra.Yaml;

public sealed class PromptYaml
{
    public string Id { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public string Template { get; set; } = string.Empty;
    public List<PromptVariableYaml> Variables { get; set; } = new();
    public List<PresetYaml> Presets { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public sealed class PromptVariableYaml
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = "string";
    public bool Required { get; set; }
    public string? Default { get; set; }
    public List<string> Options { get; set; } = new();
}

public sealed class PresetYaml
{
    public string Name { get; set; } = string.Empty;
    public Dictionary<string, string> Values { get; set; } =
        new(StringComparer.OrdinalIgnoreCase);
}

public sealed class PresetOverrideYaml
{
    public List<PresetYaml> Presets { get; set; } = new();
}
