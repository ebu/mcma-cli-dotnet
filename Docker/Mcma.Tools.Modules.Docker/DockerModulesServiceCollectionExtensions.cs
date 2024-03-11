using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Modules.Docker;

public static class DockerModulesServiceCollectionExtensions
{
    public static IServiceCollection AddDockerImageFunctionPackaging(this IServiceCollection services)
        => services.AddSingleton<IDockerCli, DockerCli>()
                   .AddSingleton<IDockerImageFunctionHelper, DockerImageFunctionHelper>();
}