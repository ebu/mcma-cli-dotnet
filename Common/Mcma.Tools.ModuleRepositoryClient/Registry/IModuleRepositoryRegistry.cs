namespace Mcma.Tools.ModuleRepositoryClient.Registry;

public interface IModuleRepositoryRegistry
{
    ModuleRepositoryRegistryEntry Get(string name);
        
    bool TryAdd(ModuleRepositoryRegistryEntry entry);

    bool TryUpdate(string name, Action<ModuleRepositoryRegistryEntry> update);
}