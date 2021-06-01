using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mcma.Client;
using Mcma.Tools.ModuleRepositoryClient.Registry;

namespace Mcma.Tools.ModuleRepositoryClient
{
    internal class ModuleRepositoryClientManager : IModuleRepositoryClientManager
    {
        public ModuleRepositoryClientManager(IModuleRepositoryRegistry registry,
                                             IAuthProvider authProvider,
                                             IEnumerable<IModuleRepositoryClientProvider> clientProviders)
        {
            Registry = registry ?? throw new ArgumentNullException(nameof(registry));
            AuthProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
            ClientProviders = clientProviders?.ToArray() ?? new IModuleRepositoryClientProvider[0];
        }

        private IModuleRepositoryRegistry Registry { get; }

        private IAuthProvider AuthProvider { get; }

        private IModuleRepositoryClientProvider[] ClientProviders { get; }

        public async Task<IModuleRepositoryClient> GetClientAsync(string name)
        {
            var repositoryEntry = Registry.Get(name);

            var clientProvider = ClientProviders.FirstOrDefault(x => x.IsSupportedUrl(repositoryEntry.Url));
            if (clientProvider == null)
                throw new Exception($"Url '{repositoryEntry.Url}' is not supported.");

            IAuthenticator authenticator = null;
            if (repositoryEntry.AuthType != null)
                authenticator = await AuthProvider.GetAsync(repositoryEntry.AuthType, repositoryEntry.AuthContext);

            return clientProvider.GetClient(repositoryEntry.Url, authenticator);
        }
    }
}