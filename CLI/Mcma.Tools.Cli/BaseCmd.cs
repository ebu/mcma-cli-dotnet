using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli;

[HelpOption]
public abstract class BaseCmd
{
    [Option("-d|--dir <DIR>", Description = "The path to use as the working directory")]
    // ReSharper disable once MemberCanBeProtected.Global - needs to be public so it can be set by CommandLineUtils
    public string WorkingDir { get; set; } = Directory.GetCurrentDirectory();
        
    public Task OnExecuteAsync(CommandLineApplication app)
    {
        WorkingDir = Path.GetFullPath(PathHelper.ExpandPath(WorkingDir), Directory.GetCurrentDirectory());
        
        Directory.SetCurrentDirectory(WorkingDir);

        if (!Directory.Exists(WorkingDir))
            throw new Exception($"Directory '{WorkingDir}' not found.");

        return ExecuteAsync(app);
    }

    protected virtual Task ExecuteAsync(CommandLineApplication app)
    {
        app.ShowHelp();
        return Task.CompletedTask;
    }
}