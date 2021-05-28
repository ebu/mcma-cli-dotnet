using System.Threading.Tasks;
using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Templates;

namespace Mcma.Tools.Modules.Dotnet.GoogleCloud
{
    public class GoogleCloudFunctionJobWorkerModuleTemplate : GoogleCloudFunctionApiModuleTemplate, INewProviderJobWorkerModuleTemplate
    {
        public GoogleCloudFunctionJobWorkerModuleTemplate(IDotnetCli dotnetCli)
            : base(dotnetCli)
        {
        }

        public Task CreateWorkerProjectAsync(NewModuleParameters moduleParameters,
                                             NewProviderModuleParameters providerParameters,
                                             string srcFolder)
            => CreateProjectAsync(moduleParameters, providerParameters, srcFolder, "mcmagcjobwrkr");
    }
}