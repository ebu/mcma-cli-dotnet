using System.Threading.Tasks;
using Mcma.Management.Modules.Templates;

namespace Mcma.Management.Modules
{
    public interface IMcmaModuleManagementTool
    {
        Task NewAsync(string template, NewModuleParameters newModuleParameters);

        Task PublishAsync(string repositoryName, Version version, string provider = null);
    }
}