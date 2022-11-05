using System;
using System.Threading.Tasks;
using Mcma.Tools.Modules;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli.Module;

[Command("pack")]
public class PackageModule : BaseCmd
{
    public PackageModule(IMcmaModulesTool modulesTool)
    {
        ModulesTool = modulesTool ?? throw new ArgumentNullException(nameof(modulesTool));
    }
        
    private IMcmaModulesTool ModulesTool { get; }
        
    [Option("-p|--provider <PROVIDER>", CommandOptionType.MultipleValue, Description = "The providers for which to package the module")]
    public Provider[] Providers { get; set; }
        
    [Option("-v|--version <VERSION>", Description = "The version of the module to package")]
    public Version Version { get; set; }

    protected override async Task ExecuteAsync(CommandLineApplication app)
    {
        if (Providers != null)
        {
            if (Version != null)
            {
                await app.Error.WriteLineAsync(
                    "Version cannot be specified when publishing to specific providers. Version should be kept the consistent across all providers.");
                return;
            }

            foreach (var provider in Providers)
                await ModulesTool.PackageProviderAsync(WorkingDir, provider);
        }
        else
            await ModulesTool.PackageAsync(WorkingDir, Version);
    }
}