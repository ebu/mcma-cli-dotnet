using Mcma.Tools.Gradle;
using Mcma.Tools.Modules.Gradle;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Cli;

public static class GradleExtensions
{
    public static IServiceCollection AddGradleModulesAndProjects(this IServiceCollection services)
        => services
           .AddGradleCli()
           .AddGradleModules();
    // .AddAwsDotnetModules()
    // .AddAzureDotnetModules()
    // .AddGoogleCloudDotnetModules();
}