using System.Text;
using PromptManager.Core.Models;

namespace PromptManager.Core.Services;

public sealed class TemplateRenderer
{
    public string Render(string template, IReadOnlyDictionary<string, string> values)
    {
        if (string.IsNullOrWhiteSpace(template))
        {
            return string.Empty;
        }

        var output = new StringBuilder(template.Length + 64);
        var index = 0;

        while (index < template.Length)
        {
            var start = template.IndexOf("{{", index, StringComparison.Ordinal);
            if (start < 0)
            {
                output.Append(template.AsSpan(index));
                break;
            }

            output.Append(template.AsSpan(index, start - index));
            var end = template.IndexOf("}}", start + 2, StringComparison.Ordinal);
            if (end < 0)
            {
                output.Append(template.AsSpan(start));
                break;
            }

            var key = template.Substring(start + 2, end - start - 2).Trim();
            if (values.TryGetValue(key, out var value))
            {
                output.Append(value);
            }
            else
            {
                output.Append("{{").Append(key).Append("}}");
            }

            index = end + 2;
        }

        return output.ToString();
    }

    public IReadOnlyList<string> Validate(Prompt prompt, IReadOnlyDictionary<string, string> values)
    {
        var missing = new List<string>();
        foreach (var variable in prompt.Variables)
        {
            if (!variable.Required)
            {
                continue;
            }

            if (!values.TryGetValue(variable.Key, out var value) || string.IsNullOrWhiteSpace(value))
            {
                missing.Add(variable.Key);
            }
        }

        return missing;
    }
}
