using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Cli
{
    [HelpOption]
    public abstract class BaseCmd
    {
        public virtual Task OnExecuteAsync(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.CompletedTask;
        }
    }
}