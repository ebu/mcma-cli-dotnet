using Mcma.Tools.ModuleRepositoryClient.Auth;

namespace Mcma.Tools.ModuleRepositoryClient
{
    internal interface IModuleRepositoryClientProvider
    {
        bool IsSupportedUrl(string url);

        IModuleRepositoryClient GetClient(string url, IModuleRepositoryAuthenticator authenticator);
    }
}