using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools;

public static class CliUtilsServiceCollectionExtensions
{
    public static IServiceCollection AddCliExecutor(this IServiceCollection services)
        => services.AddSingleton<ICliExecutor, CliExecutor>();
}