using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli
{
    [Command("mcma")]
    [Subcommand(typeof(New.New),
                typeof(Project.Project),
                typeof(Module.Module))]
    public class McmaCli : BaseCmd
    {
    }
}