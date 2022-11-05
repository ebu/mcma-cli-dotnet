using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Dotnet.Azure
{
    public class AzureFunctionAppJobWorkerModuleTemplate : AzureFunctionAppModuleTemplate
    {
        public AzureFunctionAppJobWorkerModuleTemplate(IDotnetProjectCreator dotnetProjectCreator)
            : base(dotnetProjectCreator)
        {
        }

        public override ModuleType ModuleType => ModuleType.JobWorker;

        public override async Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters)
        {
            await DotnetProjectCreator.CreateProjectAsync(parameters, srcFolder, "mcmazapi");
            await DotnetProjectCreator.CreateProjectAsync(parameters, srcFolder, "mcmazjobwrkr", true);
        }
    }
}