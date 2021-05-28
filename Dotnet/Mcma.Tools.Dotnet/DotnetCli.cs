using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mcma.Management.Utils
{
    internal class DotnetCli : IDotnetCli
    {
        public DotnetCli(ICmdExecutor cmdExecutor)
        {
            CmdExecutor = cmdExecutor ?? throw new ArgumentNullException(nameof(cmdExecutor));
        }
        
        private ICmdExecutor CmdExecutor { get; }

        public Task<(string stdOut, string stdErr)> RunCmdWithOutputAsync(string cmd, params string[] args)
            => RunCmdAsync(cmd, args, true);

        public Task<(string stdOut, string stdErr)> RunCmdWithoutOutputAsync(string cmd, params string[] args)
            => RunCmdAsync(cmd, args, false);

        public Task<(string stdOut, string stdErr)> RunCmdAsync(string cmd, string[] args, bool showOutput)
            => CmdExecutor.ExecuteAsync("dotnet", new[] { cmd }.Concat(args).ToArray(), showOutput);
    }
}