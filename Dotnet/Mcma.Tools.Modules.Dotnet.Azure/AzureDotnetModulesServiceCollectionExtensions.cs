using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules.Dotnet.Azure;

public static class AzureDotnetModulesServiceCollectionExtensions
{
    public static IServiceCollection AddAzureDotnetModules(this IServiceCollection services)
        => services.AddSingleton<IDotnetFunctionPackager, AzureFunctionAppPackager>()
                   .AddSingleton<IDotnetNewProviderModuleTemplate, AzureFunctionAppApiModuleTemplate>()
                   .AddSingleton<IDotnetNewProviderModuleTemplate, AzureFunctionAppWorkerModuleTemplate>()
                   .AddSingleton<IDotnetNewProviderModuleTemplate, AzureFunctionAppJobWorkerModuleTemplate>();
}