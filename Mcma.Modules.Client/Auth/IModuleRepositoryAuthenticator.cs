using System.Net.Http;
using System.Threading.Tasks;

namespace Mcma.Management.Modules.Auth
{
    public interface IModuleRepositoryAuthenticator
    {
        Task AddAuthenticationAsync(HttpRequestMessage request);
    }
}