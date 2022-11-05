using Mcma.Client.Auth;
using Mcma.Tools.ModuleRepositoryClient.Registry;

namespace Mcma.Tools.ModuleRepositoryClient;

internal interface IModuleRepositoryClientProvider
{
    bool IsSupportedUrl(string url);

    IModuleRepositoryClient GetClient(ModuleRepositoryRegistryEntry entry, IAuthenticator authenticator);
}