using System.Threading.Tasks;
using Mcma.Tools.Dotnet;
using Mcma.Tools.Modules.Templates;

namespace Mcma.Tools.Modules.Dotnet.GoogleCloud
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