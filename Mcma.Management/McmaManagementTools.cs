using System;
using Mcma.Management.Modules;
using Mcma.Management.Projects;

namespace Mcma.Management
{
    internal class McmaManagementTools : IMcmaManagementTools
    {
        public McmaManagementTools(IMcmaModuleManagementTool module, IMcmaProjectManagementTool project)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }

        public IMcmaModuleManagementTool Module { get; }

        public IMcmaProjectManagementTool Project { get; }
    }
}