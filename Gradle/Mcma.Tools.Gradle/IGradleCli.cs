using System.Threading.Tasks;

namespace Mcma.Tools.Gradle
{
    public interface IGradleCli
    {
        Task InstallWrapperAsync(string gradleVersion = null, string distributionType = null);
    }
}