using Mcma.Management.Modules;
using Mcma.Management.Projects;

namespace Mcma.Management
{
    public interface IMcmaManagementTools
    {
        IMcmaModuleManagementTool Module { get; }
        
        IMcmaProjectManagementTool Project { get; }
    }
}