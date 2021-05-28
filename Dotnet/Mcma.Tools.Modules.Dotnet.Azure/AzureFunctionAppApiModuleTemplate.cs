using System.Threading.Tasks;
using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Templates;

namespace Mcma.Tools.Modules.Dotnet.Azure
{
    public class AzureFunctionAppApiModuleTemplate : AzureFunctionAppModuleTemplate, INewProviderApiModuleTemplate
    {
        public AzureFunctionAppApiModuleTemplate(IDotnetCli dotnetCli)
            : base(dotnetCli)
        {
        }

        public Task CreateApiProjectAsync(NewModuleParameters moduleParameters,
                                          NewProviderModuleParameters providerParameters,
                                          string srcFolder)
            => CreateProjectAsync(moduleParameters, providerParameters, srcFolder, "mcmaazapi");
    }
}