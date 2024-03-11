using System.Collections.Concurrent;
using Mcma.Client.Auth;
using Mcma.Client.Http;
using Mcma.Tools.ModuleRepositoryClient.Registry;

namespace Mcma.Tools.ModuleRepositoryClient.Http;

internal class HttpModuleRepositoryClientProvider : IModuleRepositoryClientProvider
{
    private ConcurrentDictionary<string, HttpClient> HttpClients { get; } = new();

    public bool IsSupportedUrl(string? url) => url?.StartsWith("http", StringComparison.OrdinalIgnoreCase) ?? false;

    private static HttpClient CreateHttpClient(ModuleRepositoryRegistryEntry entry)
        => new(entry.Properties?.ToObject<HttpClientHandler>() ?? new HttpClientHandler());

    public IModuleRepositoryClient GetClient(ModuleRepositoryRegistryEntry entry, IAuthenticator? authenticator)
    {
        var httpClient = HttpClients.GetOrAdd(entry.Name, _ => CreateHttpClient(entry));
        
        return new HttpModuleRepositoryClient(new McmaHttpClient(httpClient, authenticator), httpClient, entry.Url);
    }
}