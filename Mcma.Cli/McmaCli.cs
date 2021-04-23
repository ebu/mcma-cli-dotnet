using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Cli
{
    [Command("mcma")]
    [Subcommand(typeof(Project.Project))]
    public class McmaCli
    {
    }
}