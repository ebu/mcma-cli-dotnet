using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mcma.Management.Modules
{
    public interface IModuleRepositoryClient
    {
        Task PublishAsync(JObject moduleJson, string modulePackageFilePath);
    }
}