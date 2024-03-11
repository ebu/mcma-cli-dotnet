using System.ComponentModel.DataAnnotations;
using Mcma.Tools.Modules;
using Mcma.Tools.Modules.Dotnet;
using Mcma.Tools.Modules.Gradle;
using McMaster.Extensions.CommandLineUtils;
using AllowedValuesAttribute = McMaster.Extensions.CommandLineUtils.AllowedValuesAttribute;
namespace Mcma.Tools.Cli.New;

[Command("module")]
public class NewModule : BaseCmd
{
    public NewModule(IMcmaModulesTool modulesTool)
    {
        ModulesTool = modulesTool ?? throw new ArgumentNullException(nameof(modulesTool));
    }
        
    private IMcmaModulesTool ModulesTool { get; }

    [Argument(0, Name = "moduleType")]
    [AllowedValues("api", "worker", "jobworker", IgnoreCase = true)]
    public required ModuleType ModuleType { get; set; }
        
    [Option("-bs|--buildSystem <BUILDSYSTEM>")]
    [AllowedValues(DotnetModuleBuildSystem.Name, GradleModuleBuildSystem.Name)]
    [Required]
    public required string BuildSystem { get; set; }

    [Option("-n|--name <NAME>")]
    [Required]
    public required string Name { get; set; }

    [Option("-ns|--namespace <NAMESPACE>")]
    [Required]
    public required string Namespace { get; set; }

    [Option("-p|--provider <PROVIDER>", CommandOptionType.MultipleValue)]
    [AllowedValues("aws", "azure", "google", "kubernetes", IgnoreCase = true)]
    [Required]
    public Provider[] Providers { get; set; } = [];

    [Option("-dn|--displayName <DISPLAYNAME>")]
    public string? DisplayName { get; set; }

    [Option("-desc|--description <DESCRIPTION>")]
    public string? Description { get; set; }

    [Option("-pa|--providerArg <ARG>", CommandOptionType.MultipleValue)]
    public string[] ProviderArgs { get; set; } = [];
        
    [Option("-o|--outputDir <OUTPUTDIR>")]
    public string? OutputDir { get; set; }
        
    [Option("-t|--jobType <JOBTYPE>")]
    public string? JobType { get; set; }

    protected override Task ExecuteAsync(CommandLineApplication app)
    {
        var providerArgNameValues = ProviderArgs.ToNameValuePairs();

        var providersWithArgs =
            Providers.Select(provider =>
                                 new NewProviderModuleParameters(
                                     provider,
                                     providerArgNameValues
                                         .Where(arg => arg.Name.StartsWith($"{provider}.", StringComparison.OrdinalIgnoreCase))
                                         .Select(arg => (arg.Name[$"{provider}.".Length..], arg.Value))
                                         .ToArray()));

        return ModulesTool.NewAsync(WorkingDir,
                                    BuildSystem,
                                    ModuleType,
                                    new NewModuleParameters(OutputDir ?? Directory.GetCurrentDirectory(),
                                                            Namespace,
                                                            Name,
                                                            providersWithArgs,
                                                            JobType,
                                                            DisplayName,
                                                            Description));
    }
}