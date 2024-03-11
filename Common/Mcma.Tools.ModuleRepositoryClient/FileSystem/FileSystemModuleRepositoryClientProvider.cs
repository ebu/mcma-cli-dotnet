using System.Text.RegularExpressions;
using Mcma.Client.Auth;
using Mcma.Tools.ModuleRepositoryClient.Registry;

namespace Mcma.Tools.ModuleRepositoryClient.FileSystem;

internal partial class FileSystemModuleRepositoryClientProvider : IModuleRepositoryClientProvider
{
    [GeneratedRegex(@"^((\/)|([A-Za-z]:)|(\\)).+")]
    private static partial Regex MyRegex();
    
    public bool IsSupportedUrl(string url) => MyRegex().IsMatch(url);

    public IModuleRepositoryClient GetClient(ModuleRepositoryRegistryEntry entry, IAuthenticator? authenticator) =>
        new FileSystemModuleRepositoryClient(entry.Url);
}