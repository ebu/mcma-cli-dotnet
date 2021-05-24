using System;
using System.Threading.Tasks;

namespace Mcma.Management.Projects
{
    internal class McmaProjectManagementTool : IMcmaProjectManagementTool
    {
        public McmaProjectManagementTool(IMcmaProjectModulesManagementTool modules)
        {
            Modules = modules ?? throw new ArgumentNullException(nameof(modules));
        }

        public IMcmaProjectModulesManagementTool Modules { get; }

        public Task NewAsync(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task DeployAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}