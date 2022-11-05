using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Projects;

public static class ProjectsToolServiceCollectionExtensions
{
    public static IServiceCollection AddMcmaProjectsTool(this IServiceCollection services)
        => services.AddSingleton<IMcmaProjectModulesTool, McmaProjectModulesTool>()
                   .AddSingleton<IMcmaProjectsTool, McmaProjectsTool>();
}