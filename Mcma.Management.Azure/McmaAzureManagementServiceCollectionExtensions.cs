using Mcma.Management.Modules.Packaging;
using Mcma.Management.Modules.Templates.API;
using Mcma.Management.Modules.Templates.JobWorker;
using Mcma.Management.Modules.Templates.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Management.Azure
{
    public static class McmaAzureManagementServiceCollectionExtensions
    {
        public static IServiceCollection AddMcmaAzureModuleManagement(this IServiceCollection services)
            => services.AddSingleton<IFunctionPackager, AzureFunctionAppPackager>()
                       .AddSingleton<INewProviderApiModuleTemplate, AzureFunctionAppApiModuleTemplate>()
                       .AddSingleton<INewProviderWorkerModuleTemplate, AzureFunctionAppWorkerModuleTemplate>()
                       .AddSingleton<INewProviderJobWorkerModuleTemplate, AzureFunctionAppJobWorkerModuleTemplate>();
    }
}