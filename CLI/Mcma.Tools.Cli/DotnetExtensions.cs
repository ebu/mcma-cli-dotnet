using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Dotnet;
using Mcma.Tools.Modules.Dotnet.Aws;
using Mcma.Tools.Modules.Dotnet.Azure;
using Mcma.Tools.Modules.Dotnet.GoogleCloud;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Cli;

public static class DotnetExtensions
{
    public static IServiceCollection AddDotnetModulesAndProjects(this IServiceCollection services)
        => services
           .AddDotnetCli()
           .AddDotnetModules()
           .AddAwsDotnetModules()
           .AddAzureDotnetModules()
           .AddGoogleCloudDotnetModules();
}