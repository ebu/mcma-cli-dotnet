using System;
using System.Threading;
using System.Threading.Tasks;
using Mcma.Management.Gradle;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Cli
{
    public abstract class GradleCmd : BaseCmd
    {
        protected GradleCmd(IGradleWrapper gradleWrapper)
        {
            GradleWrapper = gradleWrapper ?? throw new ArgumentNullException(nameof(gradleWrapper));
        }
        
        protected IGradleWrapper GradleWrapper { get; }
        
        protected abstract string TaskName { get; }
        
        protected abstract string[] TaskArgs { get; } 

        private async Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken cancellationToken = default)
        {
            await GradleWrapper.ExecuteTaskAsync(TaskName, TaskArgs);
        }
    }
}