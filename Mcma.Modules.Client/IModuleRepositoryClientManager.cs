namespace Mcma.Management.Modules
{
    public interface IModuleRepositoryClientManager
    {
        IModuleRepositoryClient GetClient(string name);
    }
}