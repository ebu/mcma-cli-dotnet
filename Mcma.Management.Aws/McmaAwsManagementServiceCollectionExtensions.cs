using Mcma.Management.Modules.Packaging;
using Mcma.Management.Modules.Templates.API;
using Mcma.Management.Modules.Templates.JobWorker;
using Mcma.Management.Modules.Templates.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Management.Aws
{
    public static class McmaAwsManagementServiceCollectionExtensions
    {
        public static IServiceCollection AddMcmaAwsModuleManagement(this IServiceCollection services)
            => services.AddSingleton<IFunctionPackager, AwsLambdaFunctionPackager>()
                       .AddSingleton<INewProviderApiModuleTemplate, AwsLambdaApiModuleTemplate>()
                       .AddSingleton<INewProviderWorkerModuleTemplate, AwsLambdaWorkerModuleTemplate>()
                       .AddSingleton<INewProviderJobWorkerModuleTemplate, AwsLambdaJobWorkerModuleTemplate>();
    }
}