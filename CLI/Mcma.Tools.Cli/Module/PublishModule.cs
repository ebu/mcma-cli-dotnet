using System;
using System.Threading.Tasks;
using Mcma.Tools.Modules;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli.Module;

[Command("publish")]
public class PublishModule : BaseCmd
{
    public PublishModule(IMcmaModulesTool modulesTool)
    {
        ModulesTool = modulesTool ?? throw new ArgumentNullException(nameof(modulesTool));
    }
        
    private IMcmaModulesTool ModulesTool { get; }
        
    [Option("-p|--provider <PROVIDER>", CommandOptionType.MultipleValue, Description = "The providers for which to publish the module")]
    public Provider[] Providers { get; set; }
        
    [Option("-v|--version <VERSION>", Description = "The version of the module to publish")]
    public Version Version { get; set; }
        
    [Option("-r|--repository <REPOSITORY>", Description = "The repository to which to publish the module")]
    public string Repository { get; set; }

    protected override async Task ExecuteAsync(CommandLineApplication app)
    {
        var repository = Repository ?? "default";

        if (Providers != null)
        {
            if (Version != null)
            {
                await app.Error.WriteLineAsync(
                    "Version cannot be specified when publishing to specific providers. Version should be kept the consistent across all providers.");
                return;
            }
                
            foreach (var provider in Providers)
                await ModulesTool.PublishProviderAsync(WorkingDir, provider, repository);
        }
        else
            await ModulesTool.PublishAsync(WorkingDir, repository, Version);
    }
}