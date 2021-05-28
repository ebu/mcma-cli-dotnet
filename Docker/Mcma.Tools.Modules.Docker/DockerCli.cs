using System;
using System.Linq;
using System.Threading.Tasks;
using Mcma.Management.Utils;

namespace Mcma.Management.Docker
{
    internal class DockerCli : IDockerCli
    {
        public DockerCli(ICmdExecutor cmdExecutor)
        {
            CmdExecutor = cmdExecutor ?? throw new ArgumentNullException(nameof(cmdExecutor));
        }
        
        private ICmdExecutor CmdExecutor { get; }
        
        public Task RunCmdAsync(string cmd, params string[] args)
            => CmdExecutor.ExecuteAsync("docker", new[] { cmd }.Concat(args).ToArray(), true);
    }
}