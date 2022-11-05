using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Dotnet.GoogleCloud;

public class GoogleCloudFunctionJobWorkerModuleTemplate : GoogleCloudFunctionModuleTemplate
{
    public GoogleCloudFunctionJobWorkerModuleTemplate(IDotnetProjectCreator dotnetProjectCreator)
        : base(dotnetProjectCreator)
    {
    }

    public override ModuleType ModuleType => ModuleType.JobWorker;

    public override Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters)
        => DotnetProjectCreator.CreateProjectAsync(parameters, srcFolder, "mcmagcjobwrkr");
}