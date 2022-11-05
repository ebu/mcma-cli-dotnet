using System.Threading.Tasks;

namespace Mcma.Tools;

public interface ICliExecutor
{
    Task<(string stdOut, string stdErr)> ExecuteAsync(string cmd, string[] args, bool showOutput, params (string, string)[] environmentVariables);
}