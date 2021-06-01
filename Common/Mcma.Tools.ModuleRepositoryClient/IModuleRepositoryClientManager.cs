using System.Threading.Tasks;

namespace Mcma.Tools.ModuleRepositoryClient
{
    public interface IModuleRepositoryClientManager
    {
        Task<IModuleRepositoryClient> GetClientAsync(string name);
    }
}