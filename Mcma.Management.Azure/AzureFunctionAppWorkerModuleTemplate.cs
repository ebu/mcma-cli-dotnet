using System.Threading.Tasks;
using Mcma.Management.Modules.Templates;
using Mcma.Management.Modules.Templates.JobWorker;
using Mcma.Management.Utils;

namespace Mcma.Management.Azure
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