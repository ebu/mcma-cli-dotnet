using System.Threading.Tasks;

namespace Mcma.Tools.Gradle;

public interface IGradleCli
{
    Task InitAsync(string projectName);
}