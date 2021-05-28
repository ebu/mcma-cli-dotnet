using Mcma.Tools.Modules.Packaging;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Management.Docker
{
    public static class DockerModulesServiceCollectionExtensions
    {
        public static IServiceCollection AddDockerModules(this IServiceCollection services)
            => services.AddSingleton<IDockerCli, DockerCli>().AddSingleton<IFunctionPackager, DockerImagePackager>();
    }
}