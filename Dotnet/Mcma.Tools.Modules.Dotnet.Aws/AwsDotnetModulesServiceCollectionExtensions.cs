using Mcma.Tools.Modules.Packaging;
using Mcma.Tools.Modules.Templates.API;
using Mcma.Tools.Modules.Templates.JobWorker;
using Mcma.Tools.Modules.Templates.Worker;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules.Dotnet.Aws
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