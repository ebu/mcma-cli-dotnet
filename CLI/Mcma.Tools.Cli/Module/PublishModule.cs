using System;
using System.Threading.Tasks;
using Mcma.Management.Modules;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Cli.Module
{
    [Command("publish")]
    public class PublishModule : BaseCmd
    {
        public PublishModule(IMcmaModuleManagementTool moduleManagementTool)
        {
            ModuleManagementTool = moduleManagementTool ?? throw new ArgumentNullException(nameof(moduleManagementTool));
        }
        
        private IMcmaModuleManagementTool ModuleManagementTool { get; }
        
        [Option("-p|--provider", CommandOptionType.MultipleValue)]
        public string[] Providers { get; set; }
        
        [Option("-v|--version", CommandOptionType.SingleOrNoValue)]
        public string Version { get; set; }
        
        [Option("-r|--repository", CommandOptionType.SingleOrNoValue)]
        public string Repository { get; set; }

        protected override async Task ExecuteAsync(CommandLineApplication app)
        {
            var repository = Repository ?? "default";
            var version = Version != null ? Management.Version.Parse(Version) : VersionHelper.FromFile()?.Next() ?? Management.Version.Initial();

            if (Providers != null)
            {
                foreach (var provider in Providers)
                    await ModuleManagementTool.PublishAsync(repository, version, provider);
            }
            else
                await ModuleManagementTool.PublishAsync(repository, version);
        }
    }
}