using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Cli.Project
{
    [Command("new")]
    public class New : BaseCmd
    {
        [Argument(0, Description = "The name of the new MCMA project to create")]
        public string Name { get; set; }
        
        [Option(CommandOptionType.SingleOrNoValue, LongName = "output", ShortName = "o", Description = "The folder in which to create the new MCMA project. The default is the current directory.")]
        public string OutputFolder { get; set; }

        private async Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken cancellationToken = default)
        {
            await app.Out.WriteAsync($"Creating new project '{Name}'...");
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            return 0;
        }
    }
}