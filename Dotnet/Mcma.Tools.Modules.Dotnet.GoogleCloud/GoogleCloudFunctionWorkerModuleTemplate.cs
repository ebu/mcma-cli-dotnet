using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Dotnet.GoogleCloud;

public class GoogleCloudFunctionWorkerModuleTemplate : GoogleCloudFunctionModuleTemplate
{
    public GoogleCloudFunctionWorkerModuleTemplate(IDotnetProjectCreator dotnetProjectCreator)
        : base(dotnetProjectCreator)
    {
    }

    public override ModuleType ModuleType => ModuleType.Worker;

    public override Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters)
        => DotnetProjectCreator.CreateProjectAsync(parameters, srcFolder, "mcmagcworker");
}