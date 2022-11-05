using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Dotnet.Aws;

public class AwsLambdaJobWorkerDotnetModuleTemplate : AwsLambdaApiDotnetModuleTemplate
{
    public AwsLambdaJobWorkerDotnetModuleTemplate(IDotnetProjectCreator dotnetProjectCreator)
        : base(dotnetProjectCreator)
    {
    }
        
    public override ModuleType ModuleType => ModuleType.Worker;

    public override async Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters)
    {
        await DotnetProjectCreator.CreateProjectAsync(parameters, srcFolder, "mcmaawsapi");
        await DotnetProjectCreator.CreateProjectAsync(parameters, srcFolder, "mcmaawsjobwrkr", true);
    }
}