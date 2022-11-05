using System.Threading.Tasks;

namespace Mcma.Tools.Projects;

public interface IMcmaProjectsTool
{
    IMcmaProjectModulesTool Modules { get; }
        
    Task NewAsync(string name);

    Task DeployAsync();
}