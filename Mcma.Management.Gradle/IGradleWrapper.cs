using System.Threading.Tasks;

namespace Mcma.Management.Gradle
{
    public interface IGradleWrapper
    {
        Task ExecuteTaskAsync(string task, params string[] args);
    }
}