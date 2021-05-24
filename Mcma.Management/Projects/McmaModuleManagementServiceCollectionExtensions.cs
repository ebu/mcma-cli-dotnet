using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Management.Projects
{
    public static class McmaProjectManagementServiceCollectionExtensions
    {
        public static IServiceCollection AddMcmaProjectManagement(this IServiceCollection services)
            => services.AddSingleton<IMcmaProjectModulesManagementTool, McmaProjectModulesManagementTool>()
                       .AddSingleton<IMcmaProjectManagementTool, McmaProjectManagementTool>();
    }
}