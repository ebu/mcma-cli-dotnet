using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Cli.Module
{
    [Command("module")]
    [Subcommand(typeof(PackageModule),
                typeof(PublishModule))]
    public class Module : BaseCmd
    {
    }
}