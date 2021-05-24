using System;
using Mcma.Management.Modules.FileSystem;
using Mcma.Management.Modules.Http;
using Mcma.Management.Modules.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Management.Modules
{
    public static class ModuleRepositoryClientServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleRepositoryClient(this IServiceCollection services,
                                                                   Action<ModuleRepositoryRegistryOptions> configure = null)
        {
            if (configure != null)
                services.Configure(configure);

            return services.AddHttpClient()
                           .AddSingleton<IModuleRepositoryRegistry, ModuleRepositoryRegistry>()
                           .AddSingleton<IModuleRepositoryClientProvider, FileSystemModuleRepositoryClientProvider>()
                           .AddSingleton<IModuleRepositoryClientProvider, HttpModuleRepositoryClientProvider>()
                           .AddSingleton<IModuleRepositoryClientManager, ModuleRepositoryClientManager>();
        }
    }
}