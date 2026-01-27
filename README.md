# Prompt Manager (pl)

CLI tool for managing a library of templated prompts with variables and versioning.

## Quick start (host)

```bash
dotnet build
dotnet run --project src/PromptManager.Cli -- list
```

If `config.yaml` is missing, `pl` creates a default config and continues.

## Quick start (Docker)

```bash
docker compose run --rm pl list
```

## Commands

- `pl list`
- `pl show <slug>`
- `pl run <slug>`
- `pl settings`

## Config

`config.yaml` (created on first run if missing)

```yaml
storage:
  mode: yaml
  repoPath: ./prompt-library

providers:
  openai:
    enabled: true
    apiKeyEnv: OPENAI_API_KEY
    defaultModel: gpt-4.1-mini
  ollama:
    enabled: true
    host: http://localhost:11434
    defaultModel: llama3.1

runtime:
  privacyMode: false
  storeRunHistory: true
  outputRetentionDays: 0
```

## Prompt storage

```
prompt-library/
  write-email/
    prompt.yaml
    presets.local.yaml
```

`presets.local.yaml` is optional and auto-created when saving local presets.

## Notes

- YAML storage uses Git for version history.
- Providers: OpenAI and Ollama are planned; integration comes next.
