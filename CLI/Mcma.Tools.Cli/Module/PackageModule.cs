using System;
using System.Threading.Tasks;
using Mcma.Tools.Modules;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Tools.Cli.Module
{
    [Command("pack")]
    public class PackageModule : BaseCmd
    {
        public PackageModule(IMcmaModulesTool modulesTool)
        {
            ModulesTool = modulesTool ?? throw new ArgumentNullException(nameof(modulesTool));
        }
        
        private IMcmaModulesTool ModulesTool { get; }
        
        [Option("-p|--provider", CommandOptionType.MultipleValue)]
        public string[] Providers { get; set; }
        
        [Option("-v|--version", CommandOptionType.SingleOrNoValue)]
        public string Version { get; set; }

        protected override async Task ExecuteAsync(CommandLineApplication app)
        {
            var version = Version != null ? Tools.Version.Parse(Version) : VersionHelper.FromFile()?.Next() ?? Tools.Version.Initial();

            if (Providers != null)
            {
                foreach (var provider in Providers)
                    await ModulesTool.PackageAsync(version, provider);
            }
            else
                await ModulesTool.PackageAsync(version);
        }
    }
}