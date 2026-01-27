using PromptManager.Core.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PromptManager.Infra.Yaml;

public sealed class ConfigStore
{
    private readonly IDeserializer _deserializer;
    private readonly ISerializer _serializer;

    public ConfigStore()
    {
        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        _serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
            .Build();
    }

    public Config Load(string path)
    {
        var content = File.ReadAllText(path);
        return _deserializer.Deserialize<Config>(content) ?? new Config();
    }

    public void Save(string path, Config config)
    {
        var yaml = _serializer.Serialize(config);
        File.WriteAllText(path, yaml);
    }

    public Config CreateDefault(string path)
    {
        var config = new Config();
        Save(path, config);
        return config;
    }
}
