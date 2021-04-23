using System;
using System.Threading.Tasks;

namespace Mcma.Management.Gradle
{
    public interface IGradleVersionProvider
    {
        Task<string> ResolveLatestVersionAsync();

        Task GetVersionAsync(string version, string destinationDir, IProgress<int> progressReporter);
    }
}