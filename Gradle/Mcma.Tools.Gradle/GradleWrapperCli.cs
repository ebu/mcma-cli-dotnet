using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Mcma.Tools.Gradle
{
    internal class GradleWrapperCli : CliBase, IGradleWrapperCli
    {
        public GradleWrapperCli(ICliExecutor cliExecutor)
            : base(cliExecutor)
        {
        }
        
        protected override string Executable => $"gradlew{(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".bat" : "")}";
        
        public Task<(string stdOut, string stdErr)> ExecuteTaskAsync(string taskName, params string[] args)
            => RunCmdWithOutputAsync(taskName, args);
    }
}