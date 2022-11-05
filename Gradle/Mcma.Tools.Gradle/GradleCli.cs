using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Mcma.Tools.Gradle
{
    internal class GradleCli : CliBase, IGradleCli
    {
        public GradleCli(ICliExecutor cliExecutor)
            : base(cliExecutor)
        {
        }
        
        protected override string Executable => $"gradlew{(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".bat" : "")}";
        
        public Task<(string stdOut, string stdErr)> ExecuteTaskAsync(string taskName, params string[] args)
            => CliExecutor.ExecuteAsync(Executable, new[] { taskName }.Concat(args).ToArray(), true);

        public Task InstallWrapperAsync(string gradleVersion = null, string distributionType = null)
        {
            var args = new List<string>();
             
            if (gradleVersion != null)
                args.Add(gradleVersion);
            if (distributionType != null)
                args.Add(distributionType);
            
            return RunCmdWithOutputAsync("wrapper", args.ToArray());
        }
    }
}