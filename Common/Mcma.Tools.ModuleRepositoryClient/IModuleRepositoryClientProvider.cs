using Mcma.Client;

namespace Mcma.Tools.ModuleRepositoryClient
{
    internal interface IModuleRepositoryClientProvider
    {
        bool IsSupportedUrl(string url);

        IModuleRepositoryClient GetClient(string url, IAuthenticator authenticator);
    }
}