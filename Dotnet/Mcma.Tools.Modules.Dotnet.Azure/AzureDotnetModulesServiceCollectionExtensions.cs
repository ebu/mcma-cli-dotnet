using Mcma.Tools.Modules.Packaging;
using Mcma.Tools.Modules.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules.Dotnet.Azure
{
    public static class AzureDotnetModulesServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureDotnetModules(this IServiceCollection services)
            => services.AddSingleton<IFunctionPackager, AzureFunctionAppPackager>()
                       .AddSingleton<INewProviderApiModuleTemplate, AzureFunctionAppApiModuleTemplate>()
                       .AddSingleton<INewProviderWorkerModuleTemplate, AzureFunctionAppWorkerModuleTemplate>()
                       .AddSingleton<INewProviderJobWorkerModuleTemplate, AzureFunctionAppJobWorkerModuleTemplate>();
    }
}