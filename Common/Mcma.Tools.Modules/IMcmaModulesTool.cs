using System.Threading.Tasks;

namespace Mcma.Tools.Modules;

public interface IMcmaModulesTool
{
    Task NewAsync(string rootFolder, string buildSystem, ModuleType moduleType, NewModuleParameters newModuleParameters);

    Task PackageProviderAsync(string rootFolder, Provider provider);

    Task PackageAsync(string rootFolder, Version version = null);

    Task PublishProviderAsync(string rootFolder, Provider provider, string repositoryName);

    Task PublishAsync(string rootFolder, string repositoryName, Version version = null);

    Task SetMcmaVersionAsync(string rootFolder, Version mcmaVersion);
}