using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli.New;

[Command(Name = "new")]
[Subcommand(typeof(NewModule))]
public class New : BaseCmd
{
}