using Mcma.Tools.Modules.Packaging;
using Mcma.Tools.Modules.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules.Dotnet.Aws
{
    public static class AwsDotnetModulesServiceCollectionExtensions
    {
        public static IServiceCollection AddAwsDotnetModules(this IServiceCollection services)
            => services.AddSingleton<IFunctionPackager, AwsLambdaFunctionPackager>()
                       .AddSingleton<INewProviderApiModuleTemplate, AwsLambdaApiModuleTemplate>()
                       .AddSingleton<INewProviderWorkerModuleTemplate, AwsLambdaWorkerModuleTemplate>()
                       .AddSingleton<INewProviderJobWorkerModuleTemplate, AwsLambdaJobWorkerModuleTemplate>();
    }
}