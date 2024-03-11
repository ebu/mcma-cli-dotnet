using Mcma.Client.Auth;
using Mcma.Serialization;
using Mcma.Tools.ModuleRepositoryClient.Registry;

namespace Mcma.Tools.ModuleRepositoryClient;

internal class ModuleRepositoryClientManager : IModuleRepositoryClientManager
{
    public ModuleRepositoryClientManager(IModuleRepositoryRegistry registry,
                                         IAuthProvider authProvider,
                                         IEnumerable<IModuleRepositoryClientProvider> clientProviders)
    {
        Registry = registry ?? throw new ArgumentNullException(nameof(registry));
        AuthProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
        ClientProviders = clientProviders.ToArray();
    }

    private IModuleRepositoryRegistry Registry { get; }

    private IAuthProvider AuthProvider { get; }

    private IModuleRepositoryClientProvider[] ClientProviders { get; }

    public void AddRepository(string name,
                              string url,
                              string? authType = null,
                              string? authContext = null,
                              IDictionary<string, string>? properties = null)
    {
        var entry = new ModuleRepositoryRegistryEntry
        {
            Name = name,
            Url = url,
            AuthType = authType,
            AuthContext = authContext,
            Properties = properties?.ToMcmaJsonObject()
        };

        if (!Registry.TryAdd(entry))
            throw new Exception($"Unable to add repository '{entry.Name}' because it has already been configured.");
    }


    public void SetRepositoryAuth(string name, string authType, string? authContext = null)
    {
        var updated = Registry.TryUpdate(name, x =>
        {
            x.AuthType = authType;
            x.AuthContext = authContext;
        });
        
        if (!updated)
            throw new Exception($"Unable to update repository '{name}' as it has not yet been configured.");
    }

    public IModuleRepositoryClient GetClient(string name)
    {
        var repositoryEntry = Registry.Get(name);

        var clientProvider = ClientProviders.FirstOrDefault(x => x.IsSupportedUrl(repositoryEntry.Url));
        if (clientProvider == null)
            throw new Exception($"Url '{repositoryEntry.Url}' is not supported.");

        IAuthenticator? authenticator = null;
        if (repositoryEntry.AuthType is not null)
            authenticator = AuthProvider.Get(repositoryEntry.AuthType, "ModuleRepository", nameof(Module));

        return clientProvider.GetClient(repositoryEntry, authenticator);
    }
}