using McMaster.Extensions.CommandLineUtils.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Cli.Parsers;

public static class ConventionServiceCollectionExtensions
{
    public static IServiceCollection AddCustomConventions(this IServiceCollection services)
        => services.AddSingleton<IValueParser, VersionParser>()
                   .AddSingleton<IValueParser, ProviderParser>();
}