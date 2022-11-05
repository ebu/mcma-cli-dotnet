using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Dotnet.Azure
{
    public class AzureFunctionAppApiModuleTemplate : AzureFunctionAppModuleTemplate
    {
        public AzureFunctionAppApiModuleTemplate(IDotnetProjectCreator dotnetProjectCreator)
            : base(dotnetProjectCreator)
        {
        }

        public override ModuleType ModuleType => ModuleType.API;

        public override Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters)
            => DotnetProjectCreator.CreateProjectAsync(parameters, srcFolder, "mcmaazapi");
    }
}