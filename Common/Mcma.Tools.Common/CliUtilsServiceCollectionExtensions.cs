using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Management.Utils
{
    public static class CliUtilsServiceCollectionExtensions
    {
        public static IServiceCollection AddCliUtils(this IServiceCollection services)
            => services.AddSingleton<ICmdExecutor, CmdExecutor>().AddSingleton<IDotnetCli, DotnetCli>();
    }
}