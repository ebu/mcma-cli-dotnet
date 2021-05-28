using System.Collections.Generic;
using System.Threading.Tasks;
using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Templates;

namespace Mcma.Tools.Modules.Dotnet
{
    public class NewWorkerModuleTemplate : NewModuleTemplate<INewProviderWorkerModuleTemplate>
    {
        public NewWorkerModuleTemplate(IDotnetCli dotnetCli, IEnumerable<INewProviderWorkerModuleTemplate> providerTemplates)
            : base(dotnetCli, providerTemplates)
        {
        }
        
        public override string Type => "Worker";

        protected override async Task CreateProjectsAsync(INewProviderWorkerModuleTemplate providerTemplate,
                                                          string srcFolder,
                                                          NewModuleParameters moduleParameters,
                                                          NewProviderModuleParameters providerParameters)
        {
            await providerTemplate.CreateApiProjectAsync(moduleParameters, providerParameters, srcFolder);
            await providerTemplate.CreateWorkerProjectAsync(moduleParameters, providerParameters, srcFolder);
        }
    }
}