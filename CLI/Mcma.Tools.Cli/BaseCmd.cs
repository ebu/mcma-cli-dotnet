using System;
using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli;

[HelpOption]
public abstract class BaseCmd
{
    [Option("-d|--dir <DIR>", Description = "The path to use as the working directory")]
    public string WorkingDir { get; set; }
        
    public Task OnExecuteAsync(CommandLineApplication app)
    {
        if (WorkingDir != null)
        {
            WorkingDir = Path.GetFullPath(PathHelper.ExpandPath(WorkingDir), Directory.GetCurrentDirectory());
            Directory.SetCurrentDirectory(WorkingDir);
        }
        else
            WorkingDir = Directory.GetCurrentDirectory();

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