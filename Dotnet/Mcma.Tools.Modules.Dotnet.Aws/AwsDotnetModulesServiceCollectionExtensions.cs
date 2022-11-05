using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules.Dotnet.Aws;

public static class AwsDotnetModulesServiceCollectionExtensions
{
    public static IServiceCollection AddAwsDotnetModules(this IServiceCollection services)
        => services.AddSingleton<IDotnetFunctionPackager, AwsLambdaDotnetFunctionPackager>()
                   .AddSingleton<IDotnetNewProviderModuleTemplate, AwsLambdaApiDotnetModuleTemplate>()
                   .AddSingleton<IDotnetNewProviderModuleTemplate, AwsLambdaWorkerDotnetModuleTemplate>()
                   .AddSingleton<IDotnetNewProviderModuleTemplate, AwsLambdaJobWorkerDotnetModuleTemplate>();
}