using PromptManager.Core.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PromptManager.Infra.Yaml;

public sealed class YamlPromptRepository
{
    private readonly IDeserializer _deserializer;
    private readonly ISerializer _serializer;

    public YamlPromptRepository()
    {
        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        _serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
            .Build();
    }

    public IReadOnlyList<Prompt> ListPrompts(string repoPath)
    {
        if (!Directory.Exists(repoPath))
        {
            return Array.Empty<Prompt>();
        }

        var directories = Directory.GetDirectories(repoPath);
        var prompts = new List<Prompt>();
        foreach (var dir in directories)
        {
            var promptFile = Path.Combine(dir, "prompt.yaml");
            if (!File.Exists(promptFile))
            {
                continue;
            }

            var prompt = LoadPrompt(promptFile, Path.Combine(dir, "presets.local.yaml"));
            if (prompt != null)
            {
                prompts.Add(prompt);
            }
        }

        return prompts;
    }

    public Prompt? GetPrompt(string repoPath, string slug)
    {
        var folder = Path.Combine(repoPath, slug);
        var promptFile = Path.Combine(folder, "prompt.yaml");
        if (!File.Exists(promptFile))
        {
            return null;
        }

        var localFile = Path.Combine(folder, "presets.local.yaml");
        return LoadPrompt(promptFile, localFile);
    }

    public void SavePrompt(string repoPath, Prompt prompt)
    {
        var folder = Path.Combine(repoPath, prompt.Slug);
        Directory.CreateDirectory(folder);

        var promptFile = Path.Combine(folder, "prompt.yaml");
        var yaml = ToYaml(prompt);
        File.WriteAllText(promptFile, _serializer.Serialize(yaml));
    }

    public void SaveLocalPresets(string repoPath, string slug, IReadOnlyList<Preset> presets)
    {
        var folder = Path.Combine(repoPath, slug);
        Directory.CreateDirectory(folder);
        var localFile = Path.Combine(folder, "presets.local.yaml");
        var payload = new PresetOverrideYaml
        {
            Presets = presets.Select(ToYaml).ToList()
        };
        File.WriteAllText(localFile, _serializer.Serialize(payload));
    }

    private Prompt? LoadPrompt(string promptFile, string localFile)
    {
        var promptYaml = _deserializer.Deserialize<PromptYaml>(File.ReadAllText(promptFile));
        if (promptYaml == null)
        {
            return null;
        }

        var localPresets = new List<PresetYaml>();
        if (File.Exists(localFile))
        {
            var localYaml = _deserializer.Deserialize<PresetOverrideYaml>(File.ReadAllText(localFile));
            if (localYaml?.Presets != null)
            {
                localPresets = localYaml.Presets;
            }
        }

        var mergedPresets = MergePresets(promptYaml.Presets, localPresets);
        return new Prompt
        {
            Id = promptYaml.Id,
            Slug = promptYaml.Slug,
            Title = promptYaml.Title,
            Description = promptYaml.Description,
            Tags = promptYaml.Tags,
            Template = promptYaml.Template,
            Variables = promptYaml.Variables.Select(ToModel).ToList(),
            Presets = mergedPresets.Select(ToModel).ToList(),
            CreatedAt = promptYaml.CreatedAt,
            UpdatedAt = promptYaml.UpdatedAt
        };
    }

    private static List<PresetYaml> MergePresets(IReadOnlyList<PresetYaml> basePresets, IReadOnlyList<PresetYaml> overrides)
    {
        var result = new Dictionary<string, PresetYaml>(StringComparer.OrdinalIgnoreCase);

        foreach (var preset in basePresets)
        {
            if (!string.IsNullOrWhiteSpace(preset.Name))
            {
                result[preset.Name] = preset;
            }
        }

        foreach (var preset in overrides)
        {
            if (!string.IsNullOrWhiteSpace(preset.Name))
            {
                result[preset.Name] = preset;
            }
        }

        return result.Values.ToList();
    }

    private static PromptVariable ToModel(PromptVariableYaml yaml)
    {
        return new PromptVariable
        {
            Key = yaml.Key,
            Label = yaml.Label,
            Type = yaml.Type,
            Required = yaml.Required,
            Default = yaml.Default,
            Options = yaml.Options
        };
    }

    private static Preset ToModel(PresetYaml yaml)
    {
        return new Preset
        {
            Name = yaml.Name,
            Values = yaml.Values
        };
    }

    private static PromptYaml ToYaml(Prompt prompt)
    {
        return new PromptYaml
        {
            Id = prompt.Id,
            Slug = prompt.Slug,
            Title = prompt.Title,
            Description = prompt.Description,
            Tags = prompt.Tags.ToList(),
            Template = prompt.Template,
            Variables = prompt.Variables.Select(ToYaml).ToList(),
            Presets = prompt.Presets.Select(ToYaml).ToList(),
            CreatedAt = prompt.CreatedAt,
            UpdatedAt = prompt.UpdatedAt
        };
    }

    private static PromptVariableYaml ToYaml(PromptVariable model)
    {
        return new PromptVariableYaml
        {
            Key = model.Key,
            Label = model.Label,
            Type = model.Type,
            Required = model.Required,
            Default = model.Default,
            Options = model.Options.ToList()
        };
    }

    private static PresetYaml ToYaml(Preset model)
    {
        return new PresetYaml
        {
            Name = model.Name,
            Values = new Dictionary<string, string>(model.Values, StringComparer.OrdinalIgnoreCase)
        };
    }
}
