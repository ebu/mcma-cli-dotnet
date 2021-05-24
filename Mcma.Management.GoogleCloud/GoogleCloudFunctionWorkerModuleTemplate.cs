using System.Threading.Tasks;
using Mcma.Management.Modules.Templates;
using Mcma.Management.Modules.Templates.Worker;
using Mcma.Management.Utils;

namespace Mcma.Management.GoogleCloud
{
    public class GoogleCloudFunctionWorkerModuleTemplate : GoogleCloudFunctionApiModuleTemplate, INewProviderWorkerModuleTemplate
    {
        public GoogleCloudFunctionWorkerModuleTemplate(IDotnetCli dotnetCli)
            : base(dotnetCli)
        {
        }

        public Task CreateWorkerProjectAsync(NewModuleParameters moduleParameters,
                                             NewProviderModuleParameters providerParameters,
                                             string srcFolder)
            => CreateProjectAsync(moduleParameters, providerParameters, srcFolder, "mcmagcworker");
    }
}