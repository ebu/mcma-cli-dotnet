using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Dotnet.GoogleCloud;

public class GoogleCloudFunctionApiModuleTemplate : GoogleCloudFunctionModuleTemplate
{
    public GoogleCloudFunctionApiModuleTemplate(IDotnetProjectCreator dotnetProjectCreator)
        : base(dotnetProjectCreator)
    {
    }

    public override ModuleType ModuleType => ModuleType.API;

    public override Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters)
        => DotnetProjectCreator.CreateProjectAsync(parameters, srcFolder, "mcmagcapi");
}