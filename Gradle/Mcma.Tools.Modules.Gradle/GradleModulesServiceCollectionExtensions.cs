using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules.Gradle;

public static class GradleModulesServiceCollectionExtensions
{
    public static IServiceCollection AddGradleModules(this IServiceCollection services)
        => services.AddSingleton<IModuleBuildSystem, GradleModuleBuildSystem>();
}