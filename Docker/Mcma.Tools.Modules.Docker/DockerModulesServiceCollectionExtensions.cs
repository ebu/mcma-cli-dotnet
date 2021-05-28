using Mcma.Management.Modules.Packaging;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Management.Docker
{
    public static class McmaDockerManagementServiceCollectionExtensions
    {
        public static IServiceCollection AddMcmaDockerImagePackaging(this IServiceCollection services)
            => services.AddSingleton<IDockerCli, DockerCli>().AddSingleton<IFunctionPackager, DockerImagePackager>();
    }
}