using Mcma.Management.Modules.Auth;

namespace Mcma.Management.Modules
{
    internal interface IModuleRepositoryClientProvider
    {
        bool IsSupportedUrl(string url);

        IModuleRepositoryClient GetClient(string url, IModuleRepositoryAuthenticator authenticator);
    }
}