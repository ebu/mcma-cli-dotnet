namespace Mcma.Tools.Modules.Gradle;

public interface IGradleNewProviderModuleTemplate
{
    ModuleType ModuleType { get; }
        
    Provider Provider { get; }

    Task CreateProjectsAsync(string srcFolder, NewModuleParameters parameters, NewProviderModuleParameters providerParameters);
}