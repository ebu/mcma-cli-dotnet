using System.Threading.Tasks;
using Mcma.Model;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.ModuleRepositoryClient;

public interface IModuleRepositoryClient
{
    Task PublishAsync(Module moduleJson, string modulePackageFilePath);

    Task<QueryResults<Module>> SearchAsync(ModuleSearchCriteria searchCriteria);
}