using Mcma.Tools.Git;
using Mcma.Tools.Modules.Packaging;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules;

public static class ModulesToolServiceCollectionExtensions
{
    public static IServiceCollection AddMcmaModulesTool(this IServiceCollection services)
        => services.AddGitCli()
                   .AddSingleton<IModulePackager, ModulePackager>()
                   .AddSingleton<IMcmaModulesTool, McmaModulesTool>();
}