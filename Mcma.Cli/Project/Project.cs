using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Cli.Project
{
    [Command("project")]
    [Subcommand(typeof(New))]
    public class Project : BaseCmd
    {
    }
}