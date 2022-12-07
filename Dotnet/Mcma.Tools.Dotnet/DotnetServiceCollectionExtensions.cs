using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Dotnet;

public static class DotnetServiceCollectionExtensions
{
    public static IServiceCollection AddDotnetCli(this IServiceCollection services)
        => services.AddSingleton<IDotnetCli, DotnetCli>();
}