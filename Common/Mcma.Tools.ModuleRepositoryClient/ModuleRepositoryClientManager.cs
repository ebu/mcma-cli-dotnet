using System;
using System.Collections.Generic;
using System.Linq;
using Mcma.Tools.ModuleRepositoryClient.Auth;
using Mcma.Tools.ModuleRepositoryClient.Registry;

namespace Mcma.Tools.ModuleRepositoryClient
{
    internal class ModuleRepositoryClientManager : IModuleRepositoryClientManager
    {
        public ModuleRepositoryClientManager(IModuleRepositoryRegistry registry,
                                             IEnumerable<IModuleRepositoryClientProvider> clientProviders,
                                             IEnumerable<IModuleRepositoryAuthenticatorProvider> authenticatorProviders)
        {
            Registry = registry ?? throw new ArgumentNullException(nameof(registry));
            ClientProviders = clientProviders?.ToArray() ?? new IModuleRepositoryClientProvider[0];
            AuthenticatorProviders = authenticatorProviders?.ToArray() ?? new IModuleRepositoryAuthenticatorProvider[0];
        }

        private IModuleRepositoryRegistry Registry { get; }

        private IModuleRepositoryClientProvider[] ClientProviders { get; }

        private IModuleRepositoryAuthenticatorProvider[] AuthenticatorProviders { get; }

        public IModuleRepositoryClient GetClient(string name)
        {
            var repositoryEntry = Registry.Get(name);

            var clientProvider = ClientProviders.FirstOrDefault(x => x.IsSupportedUrl(repositoryEntry.Url));
            if (clientProvider == null)
                throw new Exception($"Url '{repositoryEntry.Url}' is not supported.");

            IModuleRepositoryAuthenticator authenticator = null;
            if (repositoryEntry.AuthType != null)
            {
                var authenticatorProvider =
                    AuthenticatorProviders.FirstOrDefault(x => x.Type.Equals(repositoryEntry.AuthType, StringComparison.OrdinalIgnoreCase));
                if (authenticatorProvider == null)
                    throw new Exception($"Auth type '{repositoryEntry.AuthType}' is not supported.");

                authenticator = authenticatorProvider.GetAuthenticator(repositoryEntry.AuthContext);
            }

            return clientProvider.GetClient(repositoryEntry.Url, authenticator);
        }
    }
}