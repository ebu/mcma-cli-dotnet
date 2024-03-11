namespace Mcma.Tools.Modules.Docker;

internal class DockerCli : CliBase, IDockerCli
{
    public DockerCli(ICliExecutor cliExecutor)
        : base(cliExecutor)
    {
    }

    protected override string Executable => "docker";
        
    public Task RunCmdAsync(string cmd, params string[] args)
        => RunCmdWithOutputAsync(cmd, args);

}