using System.Text.RegularExpressions;
using Mcma.Client.Auth;
using Mcma.Tools.ModuleRepositoryClient.Registry;

namespace Mcma.Tools.ModuleRepositoryClient.FileSystem;

internal class FileSystemModuleRepositoryClientProvider : IModuleRepositoryClientProvider
{
    public bool IsSupportedUrl(string url) => Regex.IsMatch(url, @"^((\/)|([A-Za-z]:)|(\\)).+");

    public IModuleRepositoryClient GetClient(ModuleRepositoryRegistryEntry entry, IAuthenticator authenticator) =>
        new FileSystemModuleRepositoryClient(entry.Url);
}