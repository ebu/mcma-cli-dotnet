using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mcma.Tools.Dotnet
{
    internal class DotnetCli : IDotnetCli
    {
        public DotnetCli(ICliExecutor cliExecutor)
        {
            CliExecutor = cliExecutor ?? throw new ArgumentNullException(nameof(cliExecutor));
        }
        
        private ICliExecutor CliExecutor { get; }

        public Task<(string stdOut, string stdErr)> RunCmdWithOutputAsync(string cmd, params string[] args)
            => RunCmdAsync(cmd, args, true);

        public Task<(string stdOut, string stdErr)> RunCmdWithoutOutputAsync(string cmd, params string[] args)
            => RunCmdAsync(cmd, args, false);

        public Task<(string stdOut, string stdErr)> RunCmdAsync(string cmd, string[] args, bool showOutput)
            => CliExecutor.ExecuteAsync("dotnet", new[] { cmd }.Concat(args).ToArray(), showOutput);
    }
}