using System.Threading.Tasks;
using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Templates;

namespace Mcma.Tools.Modules.Dotnet.Azure
{
    public class AzureFunctionAppJobWorkerModuleTemplate : AzureFunctionAppApiModuleTemplate, INewProviderJobWorkerModuleTemplate
    {
        public AzureFunctionAppJobWorkerModuleTemplate(IDotnetCli dotnetCli)
            : base(dotnetCli)
        {
        }

        public Task CreateWorkerProjectAsync(NewModuleParameters moduleParameters,
                                             NewProviderModuleParameters providerParameters,
                                             string srcFolder)
            => CreateProjectAsync(moduleParameters, providerParameters, srcFolder, "mcmazjobwrkr", true);
    }
}