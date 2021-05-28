namespace Mcma.Tools.ModuleRepositoryClient.Registry
{
    public interface IModuleRepositoryRegistry
    {
        ModuleRepositoryRegistryEntry Get(string name);
    }
}