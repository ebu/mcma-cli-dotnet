using Mcma.Tools.Modules.Packaging;
using Mcma.Tools.Modules.Publishing;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules
{
    public static class McmaModuleManagementServiceCollectionExtensions
    {
        public static IServiceCollection AddMcmaModulesTool(this IServiceCollection services)
            => services.AddSingleton<IModulePackager, ModulePackager>()
                       .AddSingleton<IModulePublisher, ModulePublisher>()
                       .AddSingleton<IMcmaModulesTool, McmaModulesTool>();
    }
}