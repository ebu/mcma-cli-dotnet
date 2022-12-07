using Mcma.Management.Docker;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules.Dotnet;

public static class DotnetModulesServiceCollectionExtensions
{
    public static IServiceCollection AddDotnetModules(this IServiceCollection services)
        => services.AddSingleton<IDotnetProjectCreator, DotnetProjectCreator>()
                   .AddSingleton<IModuleBuildSystem, DotnetModuleBuildSystem>()
                   .AddSingleton<IDotnetFunctionPackager, DotnetDockerImageFunctionPackager>()
                   .AddDockerImageFunctionPackaging();
}