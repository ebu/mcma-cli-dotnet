using System.Threading.Tasks;
using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Templates;

namespace Mcma.Tools.Modules.Dotnet.Azure
{
    public class AzureFunctionAppWorkerModuleTemplate : AzureFunctionAppApiModuleTemplate, INewProviderJobWorkerModuleTemplate
    {
        public AzureFunctionAppWorkerModuleTemplate(IDotnetCli dotnetCli)
            : base(dotnetCli)
        {
        }

        public Task CreateWorkerProjectAsync(NewModuleParameters moduleParameters,
                                             NewProviderModuleParameters providerParameters,
                                             string srcFolder)
            => CreateProjectAsync(moduleParameters, providerParameters, srcFolder, "mcmazworker", true);
    }
}