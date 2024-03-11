namespace Mcma.Tools.Modules.Dotnet.Aws;

public class AwsLambdaApiDotnetModuleTemplate : AwsLambdaFunctionDotnetModuleTemplate
{
    public AwsLambdaApiDotnetModuleTemplate(IDotnetProjectCreator dotnetProjectCreator)
        : base(dotnetProjectCreator)
    {
    }

    public override ModuleType ModuleType => ModuleType.API;

    public override Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters)
        => DotnetProjectCreator.CreateProjectAsync(parameters, srcFolder, "mcmaawsapi");
}