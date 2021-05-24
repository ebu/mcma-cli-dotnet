using Mcma.Management.Modules.Packaging;
using Mcma.Management.Modules.Publishing;
using Mcma.Management.Modules.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Management.Modules
{
    public static class McmaModuleManagementServiceCollectionExtensions
    {
        public static IServiceCollection AddMcmaModuleManagement(this IServiceCollection services)
            => services.AddModuleTemplates()
                       .AddSingleton<IModulePackager, ModulePackager>()
                       .AddSingleton<IModulePublisher, ModulePublisher>()
                       .AddSingleton<IMcmaModuleManagementTool, McmaModuleManagementTool>();
    }
}