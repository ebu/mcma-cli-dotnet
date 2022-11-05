using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mcma.Tools.Git
{
    public class GitCli : CliBase, IGitCli
    {
        public GitCli(ICliExecutor cliExecutor)
            : base(cliExecutor)
        {
        }

        protected override string Executable => "git";

        public Task InitAsync(string dir) => RunCmdWithoutOutputAsync("init", dir);

        public Task AddAsync(string path) => RunCmdWithoutOutputAsync("add", path);
    }

    public static class GitServiceCollectionExtensions
    {
        public static IServiceCollection AddGitCli(this IServiceCollection services)
            => services.AddSingleton<IGitCli, GitCli>();
    }
}