using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli.Module
{
    [Command("module")]
    [Subcommand(typeof(PackageModule),
                typeof(PublishModule))]
    public class Module : BaseCmd
    {
    }
}