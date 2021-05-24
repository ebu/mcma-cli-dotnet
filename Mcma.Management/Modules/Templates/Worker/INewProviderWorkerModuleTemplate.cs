using System.Threading.Tasks;
using Mcma.Management.Modules.Templates.API;

namespace Mcma.Management.Modules.Templates.Worker
{
    public interface INewProviderWorkerModuleTemplate : INewProviderApiModuleTemplate
    {
        Task CreateWorkerProjectAsync(NewModuleParameters moduleParameters, NewProviderModuleParameters providerParameters, string srcFolder);
    }
}