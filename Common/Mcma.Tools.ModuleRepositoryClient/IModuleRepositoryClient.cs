using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mcma.Tools.ModuleRepositoryClient
{
    public interface IModuleRepositoryClient
    {
        Task PublishAsync(JObject moduleJson, string modulePackageFilePath);
    }
}