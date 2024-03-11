using System.Diagnostics;

namespace Mcma.Tools;

internal class CliExecutor : ICliExecutor
{
    private static string? GetFullPath(string executable)
    {
        ArgumentNullException.ThrowIfNull(executable);

        if (File.Exists(executable))
            return Path.GetFullPath(executable);
        
        var pathEnvVar = Environment.GetEnvironmentVariable("PATH") ?? "";
        var paths = pathEnvVar.Split(Path.PathSeparator);
        var fullPaths = paths.Select(path => Path.Combine(path, executable));
        
        return fullPaths.FirstOrDefault(File.Exists);
    }
    
    public Task<(string stdOut, string stdErr)> ExecuteAsync(string executable, string[] args, bool showOutput, params (string, string)[] environmentVariables)
    {
        var taskCompletionSource = new TaskCompletionSource<(string stdOut, string stdErr)>();

        var resolvedExecutable = GetFullPath(executable);
        if (resolvedExecutable == null)
            throw new CliExecutableNotFoundException(executable);

        var argsStr = string.Join(" ", args.Select(x => x.Contains(' ') ? $"\"{x}\"" : x));

        var startInfo = new ProcessStartInfo(resolvedExecutable, argsStr.Trim());
        if (!showOutput)
        {
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
        }

        var process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

        process.Exited += async (_, _) =>
        {
            try
            {
                var exitCode = process.ExitCode;
                var stdOut = !showOutput ? await process.StandardOutput.ReadToEndAsync() : string.Empty;
                var stdErr = !showOutput ? await process.StandardError.ReadToEndAsync() : string.Empty;

                if (exitCode != 0)
                    taskCompletionSource.SetException(new Exception($"Process exited with code {exitCode}.{Environment.NewLine}StdErr:{Environment.NewLine}{stdErr}"));
                else
                    taskCompletionSource.SetResult((stdOut, stdErr));
            }
            catch (Exception ex)
            {
                taskCompletionSource.SetException(ex);
            }
            finally
            {
                process.Dispose();
            }
        };

        Console.WriteLine("Executing '" + executable + " " + string.Join(" ", args) + "'...");

        process.Start();

        return taskCompletionSource.Task;
    }
}