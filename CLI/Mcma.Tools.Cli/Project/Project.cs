using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli.Project;

[Command("project")]
[Subcommand(typeof(AddModule))]
public class Project : BaseCmd
{
}