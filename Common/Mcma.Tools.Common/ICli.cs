namespace Mcma.Tools;

public abstract class CliBase
{
    protected CliBase(ICliExecutor cliExecutor)
    {
        CliExecutor = cliExecutor ?? throw new ArgumentNullException(nameof(cliExecutor));
    }
        
    protected ICliExecutor CliExecutor { get; }
        
    protected abstract string Executable { get; }

    private Task<(string stdOut, string stdErr)> RunCmdAsync(string cmd, string[] args, bool showOutput)
        => CliExecutor.ExecuteAsync(Executable, new[] { cmd }.Concat(args).ToArray(), showOutput);
        
    protected Task<(string stdOut, string stdErr)> RunCmdWithOutputAsync(string cmd, params string[] args)
        => RunCmdAsync(cmd, args, true);

    protected Task<(string stdOut, string stdErr)> RunCmdWithoutOutputAsync(string cmd, params string[] args)
        => RunCmdAsync(cmd, args, false);
}