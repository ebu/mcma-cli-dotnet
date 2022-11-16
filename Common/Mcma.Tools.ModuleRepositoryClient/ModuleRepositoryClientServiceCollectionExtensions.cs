using System;
using Mcma.Client;
using Mcma.Tools.ModuleRepositoryClient.FileSystem;
using Mcma.Tools.ModuleRepositoryClient.Http;
using Mcma.Tools.ModuleRepositoryClient.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.ModuleRepositoryClient;

public static class ModuleRepositoryClientServiceCollectionExtensions
{
    public static IServiceCollection AddModuleRepositoryClient(this IServiceCollection services,
                                                               Action<ModuleRepositoryRegistryOptions> configure = null)
    {
        if (configure != null)
            services.Configure(configure);

        return services.AddMcmaClient(x => x.Auth.AddModuleRepositoryAuth())
                       .AddSingleton<IModuleRepositoryRegistry, ModuleRepositoryRegistry>()
                       .AddSingleton<IModuleRepositoryClientProvider, FileSystemModuleRepositoryClientProvider>()
                       .AddSingleton<IModuleRepositoryClientProvider, HttpModuleRepositoryClientProvider>()
                       .AddSingleton<IModuleRepositoryClientManager, ModuleRepositoryClientManager>();
    }
}