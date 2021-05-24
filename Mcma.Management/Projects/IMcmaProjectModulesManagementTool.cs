using System.Threading.Tasks;

namespace Mcma.Management.Projects
{
    public interface IMcmaProjectModulesManagementTool
    {
        Task AddModuleAsync(string @namespace, string name, string provider, Version version);

        Task AddLocalModuleAsync(string name, string provider);
    }
}