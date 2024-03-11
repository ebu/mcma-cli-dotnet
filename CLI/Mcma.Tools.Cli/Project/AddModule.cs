using Mcma.Tools.Projects;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli.Project;

[Command("add-module")]
public class AddModule : BaseCmd
{
    public AddModule(IMcmaProjectsTool projectsTool)
    {
        ProjectsTool = projectsTool ?? throw new ArgumentNullException(nameof(projectsTool));
    }

    private IMcmaProjectsTool ProjectsTool { get; }
        
    protected override Task ExecuteAsync(CommandLineApplication app)
    {
        // TODO: Add module to project using ProjectsTool

        ProjectsTool.Modules.AddModuleAsync("", "", "", new Version(1, 0, 0));
        
        return Task.CompletedTask;
    }
}