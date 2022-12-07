using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Gradle;

public static class GradleServiceCollectionExtensions
{
    public static IServiceCollection AddGradleCli(this IServiceCollection services)
        => services.AddSingleton<IGradleCli, GradleCli>()
                   .AddSingleton<IGradleWrapperCli, GradleWrapperCli>();
}