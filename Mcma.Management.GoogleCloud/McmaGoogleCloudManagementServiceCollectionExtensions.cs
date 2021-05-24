using Mcma.Management.Modules.Packaging;
using Mcma.Management.Modules.Templates.API;
using Mcma.Management.Modules.Templates.JobWorker;
using Mcma.Management.Modules.Templates.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Management.GoogleCloud
{
    public static class McmaGoogleCloudManagementServiceCollectionExtensions
    {
        public static IServiceCollection AddMcmaGoogleCloudModuleManagement(this IServiceCollection services)
            => services.AddSingleton<IFunctionPackager, GoogleCloudFunctionPackager>()
                       .AddSingleton<INewProviderApiModuleTemplate, GoogleCloudFunctionApiModuleTemplate>()
                       .AddSingleton<INewProviderWorkerModuleTemplate, GoogleCloudFunctionWorkerModuleTemplate>()
                       .AddSingleton<INewProviderJobWorkerModuleTemplate, GoogleCloudFunctionJobWorkerModuleTemplate>();
    }
}