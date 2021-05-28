namespace Mcma.Tools.ModuleRepositoryClient
{
    public interface IModuleRepositoryClientManager
    {
        IModuleRepositoryClient GetClient(string name);
    }
}