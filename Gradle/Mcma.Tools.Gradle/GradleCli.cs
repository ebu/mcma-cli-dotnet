using System;
using System.Threading.Tasks;

namespace Mcma.Tools.Gradle;

internal class GradleCli : CliBase, IGradleCli
{
    public GradleCli(ICliExecutor cliExecutor)
        : base(cliExecutor)
    {
    }

    protected override string Executable => "gradle.bat";

    public async Task InitAsync(string projectName)
    {
        try
        {
            await RunCmdWithOutputAsync("init", "--type=basic", "--dsl=groovy", $"--project-name={projectName}");
        }
        catch (CliExecutableNotFoundException)
        {
            Console.WriteLine("Gradle is not installed. Please follow the instructions to install it here:");
            Console.WriteLine("https://gradle.org/install/");
            Environment.Exit(1);
        }
    }
}