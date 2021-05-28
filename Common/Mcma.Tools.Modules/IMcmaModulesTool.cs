using System.Threading.Tasks;
using Mcma.Tools.Modules.Templates;

namespace Mcma.Tools.Modules
{
    public interface IMcmaModulesTool
    {
        Task NewAsync(string template, NewModuleParameters newModuleParameters);

        Task PackageAsync(Version version, string provider = null);

        Task PublishAsync(string repositoryName, Version version, string provider = null);
    }
}