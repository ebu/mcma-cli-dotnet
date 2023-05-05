using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mcma.Tools;

public class CliExecutableNotFoundException : Exception
{
    public CliExecutableNotFoundException(string executable)
        : base(
            $"'{executable}' was not found on any of the paths in the PATH variable. " +
            "Please ensure that it's properly installed in a folder registered in PATH or specify the full path.")
    {
    }
}

internal class CliExecutor : ICliExecutor
{
    private static string GetFullPath(string executable)
    {
        if (executable == null)
            throw new ArgumentNullException(nameof(executable));
        
        return File.Exists(executable)
            ? Path.GetFullPath(executable)
            : (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator).Select(p => Path.Combine(p, executable)).FirstOrDefault(File.Exists);
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