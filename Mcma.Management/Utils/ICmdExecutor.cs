using System.Threading.Tasks;

namespace Mcma.Management.Utils
{
    public interface ICmdExecutor
    {
        Task<(string stdOut, string stdErr)> ExecuteAsync(string cmd, string[] args, bool showOutput, params (string, string)[] environmentVariables);
    }
}