using System.Threading.Tasks;

namespace Mcma.Management.Docker
{
    public interface IDockerCli
    {
        Task RunCmdAsync(string cmd, params string[] args);
    }
}