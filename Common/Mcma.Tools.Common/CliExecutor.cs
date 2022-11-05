using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Mcma.Tools;

internal class CliExecutor : ICliExecutor
{    
    public Task<(string stdOut, string stdErr)> ExecuteAsync(string cmd, string[] args, bool showOutput, params (string, string)[] environmentVariables)
    {
        var taskCompletionSource = new TaskCompletionSource<(string stdOut, string stdErr)>();

        var startInfo = new ProcessStartInfo(cmd, string.Join(" ", args));
        if (!showOutput)
        {
            startInfo.UseShellExecute = false;
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
            
        Console.WriteLine("Executing '" + cmd + " " + string.Join(" ", args) + "'...");
    
        process.Start();
    
        return taskCompletionSource.Task;
    }
}

class CliExecutorImpl : CliExecutor
{
}