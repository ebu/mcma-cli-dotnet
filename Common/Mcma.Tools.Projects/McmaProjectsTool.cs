using System;
using System.Threading.Tasks;

namespace Mcma.Tools.Projects
{
    internal class McmaProjectsTool : IMcmaProjectsTool
    {
        public McmaProjectsTool(IMcmaProjectModulesTool modules)
        {
            Modules = modules ?? throw new ArgumentNullException(nameof(modules));
        }

        public IMcmaProjectModulesTool Modules { get; }

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