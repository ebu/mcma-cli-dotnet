namespace Mcma.Tools.Modules.Docker;

public interface IDockerCli
{
    Task RunCmdAsync(string cmd, params string[] args);
}