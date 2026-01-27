using PromptManager.Cli;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.SetApplicationName("pl");
    config.PropagateExceptions();
    config.Settings.HelpProviderStyles = null;

    config.AddCommand<PromptListCommand>("list")
        .WithDescription("List prompts");

    config.AddCommand<PromptShowCommand>("show")
        .WithDescription("Show a prompt")
        .WithExample(new[] { "show", "write-email" });

    config.AddCommand<PromptRunCommand>("run")
        .WithDescription("Run a prompt")
        .WithExample(new[] { "run", "write-email" });

    config.AddCommand<SettingsShowCommand>("settings")
        .WithDescription("Show current settings");
});

return app.Run(args);
