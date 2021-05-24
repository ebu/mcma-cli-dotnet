namespace Mcma.Management.Modules.Registry
{
    public interface IModuleRepositoryRegistry
    {
        ModuleRepositoryRegistryEntry Get(string name);
    }
}