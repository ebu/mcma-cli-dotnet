using Mcma.Tools.Modules.Packaging;
using Mcma.Tools.Modules.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules.Dotnet.GoogleCloud
{
    public static class GoogleCloudDotnetModulesServiceCollectionExtensions
    {
        public static IServiceCollection AddGoogleCloudDotnetModules(this IServiceCollection services)
            => services.AddSingleton<IFunctionPackager, GoogleCloudFunctionPackager>()
                       .AddSingleton<INewProviderApiModuleTemplate, GoogleCloudFunctionApiModuleTemplate>()
                       .AddSingleton<INewProviderWorkerModuleTemplate, GoogleCloudFunctionWorkerModuleTemplate>()
                       .AddSingleton<INewProviderJobWorkerModuleTemplate, GoogleCloudFunctionJobWorkerModuleTemplate>();
    }
}