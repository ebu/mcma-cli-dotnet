using System.Threading.Tasks;

namespace Mcma.Management.Utils
{
    public interface IDotnetCli
    {
        Task<(string stdOut, string stdErr)> RunCmdWithOutputAsync(string cmd, params string[] args);

        Task<(string stdOut, string stdErr)> RunCmdWithoutOutputAsync(string cmd, params string[] args);

        Task<(string stdOut, string stdErr)> RunCmdAsync(string cmd, string[] args, bool showOutput);
    }
}