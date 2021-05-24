using System.Threading.Tasks;
using Mcma.Management.Modules.Templates;
using Mcma.Management.Modules.Templates.API;
using Mcma.Management.Utils;

namespace Mcma.Management.GoogleCloud
{
    public class GoogleCloudFunctionApiModuleTemplate : GoogleCloudFunctionModuleTemplate, INewProviderApiModuleTemplate
    {
        public GoogleCloudFunctionApiModuleTemplate(IDotnetCli dotnetCli)
            : base(dotnetCli)
        {
        }

        public Task CreateApiProjectAsync(NewModuleParameters moduleParameters,
                                          NewProviderModuleParameters providerParameters,
                                          string srcFolder)
            => CreateProjectAsync(moduleParameters, providerParameters, srcFolder, "mcmagcapi");
    }
}