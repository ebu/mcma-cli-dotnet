using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Git;

public static class GitServiceCollectionExtensions
{
    public static IServiceCollection AddGitCli(this IServiceCollection services)
        => services.AddSingleton<IGitCli, GitCli>();
}