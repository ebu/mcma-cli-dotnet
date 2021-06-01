using System.Text.RegularExpressions;
using Mcma.Client;

namespace Mcma.Tools.ModuleRepositoryClient.FileSystem
{
    internal class FileSystemModuleRepositoryClientProvider : IModuleRepositoryClientProvider
    {
        public bool IsSupportedUrl(string url) => Regex.IsMatch(url, @"^((\/)|([A-Za-z]:)|(\\)).+");

        public IModuleRepositoryClient GetClient(string url, IAuthenticator authenticator) => new FileSystemModuleRepositoryClient(url);
    }
}