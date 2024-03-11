using Mcma.Client;
using Mcma.Tools.ModuleRepositoryClient.Auth;
using Mcma.Tools.ModuleRepositoryClient.FileSystem;
using Mcma.Tools.ModuleRepositoryClient.Http;
using Mcma.Tools.ModuleRepositoryClient.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.ModuleRepositoryClient;

public static class ModuleRepositoryClientServiceCollectionExtensions
{
    public static IServiceCollection AddModuleRepositoryClient(this IServiceCollection services,
                                                               Action<ModuleRepositoryRegistryOptions>? configure = null)
    {
        if (configure is not null)
            services.Configure(configure);

        return services.AddSingletonMcmaClient(x => x.AddAuth(auth => auth.AddModuleRepositoryAuth()))
                       .AddSingleton<IModuleRepositoryRegistry, ModuleRepositoryRegistry>()
                       .AddSingleton<IModuleRepositoryClientProvider, FileSystemModuleRepositoryClientProvider>()
                       .AddSingleton<IModuleRepositoryClientProvider, HttpModuleRepositoryClientProvider>()
                       .AddSingleton<IModuleRepositoryClientManager, ModuleRepositoryClientManager>();
    }
}