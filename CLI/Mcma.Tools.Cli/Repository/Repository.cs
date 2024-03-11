using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli.Repository;

[Command("repository")]
[Subcommand(typeof(AddRepository),
            typeof(SetRepositoryAuth))]
public class Repository : BaseCmd;