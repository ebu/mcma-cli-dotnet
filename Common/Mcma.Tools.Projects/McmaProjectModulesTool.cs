using System.Threading.Tasks;

namespace Mcma.Tools.Projects;

internal class McmaProjectModulesTool : IMcmaProjectModulesTool
{
    public Task AddModuleAsync(string @namespace, string name, string provider, Version version)
    {
        throw new System.NotImplementedException();
    }

    public Task AddLocalModuleAsync(string name, string provider)
    {
        throw new System.NotImplementedException();
    }
}