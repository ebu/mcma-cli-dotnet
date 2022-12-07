using System;
using System.Linq;
using System.Threading.Tasks;
using Mcma.Tools;

namespace Mcma.Management.Docker;

internal class DockerCli : IDockerCli
{
    public DockerCli(ICliExecutor cmdExecutor)
    {
        CliExecutor = cmdExecutor ?? throw new ArgumentNullException(nameof(cmdExecutor));
    }
        
    private ICliExecutor CliExecutor { get; }
        
    public Task RunCmdAsync(string cmd, params string[] args)
        => CliExecutor.ExecuteAsync("docker", new[] { cmd }.Concat(args).ToArray(), true);
}