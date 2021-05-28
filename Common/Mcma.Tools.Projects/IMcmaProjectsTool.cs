using System.Threading.Tasks;

namespace Mcma.Management.Projects
{
    public interface IMcmaProjectManagementTool
    {
        IMcmaProjectModulesManagementTool Modules { get; }
        
        Task NewAsync(string name);

        Task DeployAsync();
    }
}