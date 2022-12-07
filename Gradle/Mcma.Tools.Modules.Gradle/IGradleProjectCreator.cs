using System.Threading.Tasks;

namespace Mcma.Tools.Modules.Gradle;

public interface IGradleProjectCreator
{
    Task CreateProjectAsync(NewModuleParameters moduleParameters,
                            string srcFolder,
                            string template,
                            bool addJobTypeArg = false);
}