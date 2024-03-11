namespace Mcma.Tools.Modules.Gradle.Aws;

public abstract class AwsLambdaFunctionGradleModuleTemplate : IGradleNewProviderModuleTemplate
{
    protected AwsLambdaFunctionGradleModuleTemplate(IGradleProjectCreator projectCreator)
    {
        ProjectCreator = projectCreator ?? throw new ArgumentNullException(nameof(projectCreator));
    }

    private IGradleProjectCreator ProjectCreator { get; }
        
    public abstract ModuleType ModuleType { get; }

    public Provider Provider => Provider.AWS;

    public Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters)

        => ProjectCreator.CreateProjectAsync(parameters, srcFolder, "");
}