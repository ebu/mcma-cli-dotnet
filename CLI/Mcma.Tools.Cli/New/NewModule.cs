using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mcma.Management.Modules;
using Mcma.Management.Modules.Templates;
using McMaster.Extensions.CommandLineUtils;

namespace Mcma.Cli.New
{
    [Command("module")]
    public class NewModule : BaseCmd
    {
        public NewModule(IMcmaModuleManagementTool moduleManagementTool)
        {
            ModuleManagementTool = moduleManagementTool;
        }
        
        private IMcmaModuleManagementTool ModuleManagementTool { get; }

        [Argument(0, Name = "template")]
        [AllowedValues("api", "worker", "jobworker", IgnoreCase = true)]
        public string Template { get; set; }

        [Option("-n|--name")]
        [Required]
        public string Name { get; set; }

        [Option("-ns|--namespace")]
        [Required]
        public string Namespace { get; set; }

        [Option("-dn|--displayName")]
        public string DisplayName { get; set; }

        [Option("-d|--description")]
        public string Description { get; set; }
        
        [Option("-p|--provider", CommandOptionType.MultipleValue)]
        [AllowedValues("aws", "azure", "google", "kubernetes", IgnoreCase = true)]
        [Required]
        public string[] Providers { get; set; }
        
        [Option("-pa|--providerArg", CommandOptionType.MultipleValue)]
        public string[] ProviderArgs { get; set; }
        
        [Option("-o|--output")]
        public string OutputDir { get; set; }
        
        [Option("-t|--jobType")]
        public string JobType { get; set; }

        private IEnumerable<NewProviderModuleParameters> ProvidersWithArgs
        {
            get
            {
                var providers = Providers ?? Array.Empty<string>();
                var providerArgs = ProviderArgs ?? Array.Empty<string>();

                if (providerArgs.Length == 0)
                    return providers.Select(p => new NewProviderModuleParameters(p));

                return providers.Select(provider =>
                                            new NewProviderModuleParameters(
                                                provider,
                                                providerArgs.Where(arg => arg.StartsWith($"{provider}.", StringComparison.OrdinalIgnoreCase))
                                                            .Select(argForProvider =>
                                                            {
                                                                var argKeyAndValue = argForProvider.Split(":");
                                                                if (argKeyAndValue.Length != 2)
                                                                    throw new Exception($"Invalid provider arg '{argForProvider}'");
                                                                return (argKeyAndValue[0][(provider.Length + 1)..], argKeyAndValue[1]);
                                                            })
                                                            .ToArray()));
            }
        }

        protected override Task ExecuteAsync(CommandLineApplication app)
            => ModuleManagementTool.NewAsync(Template,
                                             new NewModuleParameters(OutputDir ?? Directory.GetCurrentDirectory(),
                                                                     Namespace,
                                                                     Name,
                                                                     ProvidersWithArgs,
                                                                     JobType,
                                                                     DisplayName,
                                                                     Description));
    }
}