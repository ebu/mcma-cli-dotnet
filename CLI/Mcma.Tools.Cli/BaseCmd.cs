using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli
{
    [HelpOption]
    public abstract class BaseCmd
    {
        [Option("-d|--dir", CommandOptionType.SingleOrNoValue, Description = "The path to use as the working directory")]
        public string WorkingDir { get; set; }
        
        public Task OnExecuteAsync(CommandLineApplication app)
        {
            if (WorkingDir != null)
                Directory.SetCurrentDirectory(WorkingDir);

            return ExecuteAsync(app);
        }

        protected virtual Task ExecuteAsync(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.CompletedTask;
        }
    }
}