using System.Text.RegularExpressions;
using Mcma.Management.Modules.Auth;

namespace Mcma.Management.Modules.FileSystem
{
    internal class FileSystemModuleRepositoryClientProvider : IModuleRepositoryClientProvider
    {
        public bool IsSupportedUrl(string url) => Regex.IsMatch(url, @"^((\/)|([A-Za-z]:)|(\\)).+");

        public IModuleRepositoryClient GetClient(string url, IModuleRepositoryAuthenticator authenticator) => new FileSystemModuleRepositoryClient(url);
    }
}