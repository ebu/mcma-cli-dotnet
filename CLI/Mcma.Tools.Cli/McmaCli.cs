using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli;

[Command("mcma")]
[Subcommand(typeof(New.New),
            typeof(Project.Project),
            typeof(Module.Module),
            typeof(Repository.Repository))]
public class McmaCli : BaseCmd;