using System.Threading.Tasks;
using Mcma.Management.DataModel;

namespace Mcma.Management
{
    public interface IBuildSystem
    {
        Task ConfigureNewProjectAsync(Project project);

        Task ConfigureNewModuleAsync(Module module);
        
        
    }
}