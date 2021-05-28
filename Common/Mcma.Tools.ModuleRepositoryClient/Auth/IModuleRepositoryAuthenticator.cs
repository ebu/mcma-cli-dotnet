using System.Net.Http;
using System.Threading.Tasks;

namespace Mcma.Tools.ModuleRepositoryClient.Auth
{
    public interface IModuleRepositoryAuthenticator
    {
        Task AddAuthenticationAsync(HttpRequestMessage request);
    }
}