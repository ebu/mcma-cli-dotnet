using Mcma.Tools.Modules;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli.Module;

[Command("set-mcma-version")]
public class SetModuleMcmaVersion : BaseCmd
{
    public SetModuleMcmaVersion(IMcmaModulesTool modulesTool)
    {
        ModulesTool = modulesTool ?? throw new ArgumentNullException(nameof(modulesTool));
    }
        
    private IMcmaModulesTool ModulesTool { get; }

    [Argument(0)]
    public required Version Version { get; set; }

    protected override Task ExecuteAsync(CommandLineApplication app)
        => ModulesTool.SetMcmaVersionAsync(WorkingDir, Version);
}