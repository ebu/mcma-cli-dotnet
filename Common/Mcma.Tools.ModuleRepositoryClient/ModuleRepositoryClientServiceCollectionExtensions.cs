using System;
using Mcma.Aws.Client;
using Mcma.Client;
using Mcma.Tools.ModuleRepositoryClient.FileSystem;
using Mcma.Tools.ModuleRepositoryClient.Http;
using Mcma.Tools.ModuleRepositoryClient.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.ModuleRepositoryClient
{
    public static class ModuleRepositoryClientServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleRepositoryClient(this IServiceCollection services,
                                                                   Action<ModuleRepositoryRegistryOptions> configure = null)
        {
            if (configure != null)
                services.Configure(configure);

            return services.AddHttpClient()
                           .AddMcmaClient(x => x.Auth.AddAws4Auth())
                           .AddSingleton<IModuleRepositoryRegistry, ModuleRepositoryRegistry>()
                           .AddSingleton<IModuleRepositoryClientProvider, FileSystemModuleRepositoryClientProvider>()
                           .AddSingleton<IModuleRepositoryClientProvider, HttpModuleRepositoryClientProvider>()
                           .AddSingleton<IModuleRepositoryClientManager, ModuleRepositoryClientManager>();
        }
    }
}