using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules.Dotnet.GoogleCloud;

public static class GoogleCloudDotnetModulesServiceCollectionExtensions
{
    public static IServiceCollection AddGoogleCloudDotnetModules(this IServiceCollection services)
        => services.AddSingleton<IDotnetFunctionPackager, GoogleCloudFunctionPackager>()
                   .AddSingleton<IDotnetNewProviderModuleTemplate, GoogleCloudFunctionApiModuleTemplate>()
                   .AddSingleton<IDotnetNewProviderModuleTemplate, GoogleCloudFunctionWorkerModuleTemplate>()
                   .AddSingleton<IDotnetNewProviderModuleTemplate, GoogleCloudFunctionJobWorkerModuleTemplate>();
}