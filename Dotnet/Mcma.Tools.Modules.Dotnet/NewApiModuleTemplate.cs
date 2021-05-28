using System.Collections.Generic;
using System.Threading.Tasks;
using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Templates;

namespace Mcma.Tools.Modules.Dotnet
{
    public class NewApiModuleTemplate : NewModuleTemplate<INewProviderApiModuleTemplate>
    {
        public NewApiModuleTemplate(IDotnetCli dotnetCli, IEnumerable<INewProviderApiModuleTemplate> providerTemplates)
            : base(dotnetCli, providerTemplates)
        {
        }
        
        public override string Type => "API";

        protected override async Task CreateProjectsAsync(INewProviderApiModuleTemplate providerTemplate,
                                                          string srcFolder,
                                                          NewModuleParameters moduleParameters,
                                                          NewProviderModuleParameters providerParameters)
        {
            await providerTemplate.CreateApiProjectAsync(moduleParameters, providerParameters, srcFolder);
        }
    }
}