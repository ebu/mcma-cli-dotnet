using System;
using Mcma.Management.Modules;
using Mcma.Management.Modules.Registry;
using Mcma.Management.Projects;
using Mcma.Management.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Management
{
    public static class McmaManagementServiceCollectionExtensions
    {
        public static IServiceCollection AddMcmaManagement(this IServiceCollection services,
                                                           Action<ModuleRepositoryRegistryOptions> configureRegistry = null)
            => services.AddModuleRepositoryClient(configureRegistry)
                       .AddCliUtils()
                       .AddMcmaModuleManagement()
                       .AddMcmaProjectManagement()
                       .AddSingleton<IMcmaManagementTools, McmaManagementTools>();
    }
}