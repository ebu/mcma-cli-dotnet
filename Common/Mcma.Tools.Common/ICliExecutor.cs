namespace Mcma.Tools;

public interface ICliExecutor
{
    Task<(string stdOut, string stdErr)> ExecuteAsync(string executable, string[] args, bool showOutput, params (string, string)[] environmentVariables);
}