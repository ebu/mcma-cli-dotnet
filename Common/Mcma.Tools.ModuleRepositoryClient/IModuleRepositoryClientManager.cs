using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mcma.Tools.ModuleRepositoryClient;

public interface IModuleRepositoryClientManager
{
    void AddRepository(string name,
                       string url,
                       string authType = null,
                       string authContext = null,
                       IDictionary<string, string> properties = null);

    void SetRepositoryAuth(string name, string authType, string authContext = null);
        
    Task<IModuleRepositoryClient> GetClientAsync(string name);
}