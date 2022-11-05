using System;
using System.Threading.Tasks;
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
        return Task.CompletedTask;
    }
}